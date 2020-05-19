using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using GraphML = Satsuma.IO.GraphML;
using Graph = Satsuma.IGraph;

namespace Backend.Models.Semantic
{
    class SemanticGraph
    {
        public Graph Graph { get; set; }
        public GraphML.StandardProperty<string> LabelsMap { get; set; }
        // contains entries: v label -> set of labels of broaders
        public Dictionary<string, HashSet<String>> Broaders = new Dictionary<string, HashSet<String>>();
        // contains entries: v label -> set of labels of narrowers
        public Dictionary<string, HashSet<String>> Narrowers = new Dictionary<string, HashSet<String>>();

        public Dictionary<string, HashSet<String>> WordLabels = new Dictionary<string, HashSet<String>>();

        private GraphML.StandardProperty<string> EdgeTypeMap { get; set; }
        // private GraphML.StandardProperty<string> NarrowerEdgesMap { get; set; }
        private string GraphPath { get; set; }
        private string NodeIdPropName { get; set; }
        private string EdgePropName { get; set; }

        private Levenshtein LevSim = new Levenshtein();

        private HashSet<string> EMPTY_SET = new HashSet<string>();

        // private Dictionary<string, List<String>> NodeNeighbLabelsDict { get; set; }
        public SemanticGraph(String graphPath, string nodeIdPropName,
            string edgePropName)
        {
            this.GraphPath = graphPath;
            this.NodeIdPropName = nodeIdPropName;
            this.EdgePropName = edgePropName;
            InitGraph();
            // remove graph to reduce amount of used memory
            // Console.WriteLine("Removing Graph object from memory as it is no longer need ... ");
            // this.Graph = null;
            // GC.Collect();
            // GC.WaitForPendingFinalizers();
        }

        /// Loads graph and initialize required graph-related structures.
        private void InitGraph()
        {
            // load graph
            GraphML.GraphMLFormat f = new GraphML.GraphMLFormat();
            f.Load(this.GraphPath);
            this.Graph = f.Graph;
            // prepare label - vertex mapping
            LabelsMap = (GraphML.StandardProperty<string>)
                f.Properties.FirstOrDefault(x => x.Name == this.NodeIdPropName &&
                    x.Domain == GraphML.PropertyDomain.Node &&
                    x is GraphML.StandardProperty<string>);
            // prepare rel type - edge mapping
            EdgeTypeMap = (GraphML.StandardProperty<string>)
                f.Properties.FirstOrDefault(x => x.Name == EdgePropName &&
                    x.Domain == GraphML.PropertyDomain.Arc &&
                    x is GraphML.StandardProperty<string>);

            this.BuildNeighborsMappings();
            this.BuildWordIndex();
        }

        // Due to lack of such basic feature as giving list of neighbors,
        // this method builds two mappings:
        //   vertex label -> labels of broader vertices
        //   vertex label -> labels of narrower vertices
        private void BuildNeighborsMappings()
        {
            // contains entries: v label -> set of labels of broaders
            // Dictionary<string, HashSet<String>> brDict = new Dictionary<string, HashSet<String>>();
            // // contains entries: v label -> set of labels of narrowers
            // Dictionary<string, HashSet<String>> nrDict = new Dictionary<string, HashSet<String>>();

            foreach (var arc in Graph.Arcs())
            {

                string src = "";
                src = LabelsMap.TryGetValue(Graph.U(arc), out src) ? src : "";
                src = src.ToLower();
                string target = "";
                target = LabelsMap.TryGetValue(Graph.V(arc), out target) ? target : "";
                target = target.ToLower();
                string broader = "";
                string narrower = "";
                string edgeType = "";
                EdgeTypeMap.TryGetValue(arc, out edgeType);
                if (edgeType == "broader")
                {
                    // src - broader -> tgt
                    narrower = src;
                    broader = target;

                } else if (edgeType == "narrower")
                {
                    // src - narrower -> tgt    =>   tgt - broader -> src
                    broader = src;
                    narrower = target;
                } else {
                    throw new Exception("Unknown relationship type found: '" + edgeType + "'");
                }
                if (narrower != null && narrower != "")
                {
                    if (!Broaders.ContainsKey(narrower))
                    {
                        Broaders.Add(narrower, new HashSet<string>());
                    }
                    Broaders[narrower].Add(broader);
                }
                if (broader != null && broader != "")
                {
                    if (!Narrowers.ContainsKey(broader))
                    {
                        Narrowers.Add(broader, new HashSet<string>());
                    }
                    Narrowers[broader].Add(narrower);
                }
            }
            Console.WriteLine("Loaded " + Broaders.Count + " broader relations");
            Console.WriteLine("Loaded " + Narrowers.Count + " narrower relations");
        }

        // Build word -> labels index. It's crucial, because of big size of graph
        // we cannot compare each word with all labels in graph
        private void BuildWordIndex(int wordMinLen = 4)
        {
            foreach (var n in Graph.Nodes())
            {
                var label = LabelsMap[n];
                if (label != null)
                {
                    label = label.ToLower();
                    var words = Regex.Split(label, @"\s");
                    foreach (var w in words)
                    {
                        if (w.Length >= wordMinLen)
                        {
                            if (!WordLabels.ContainsKey(w))
                            {
                                WordLabels.Add(w, new HashSet<string>());
                            }
                            WordLabels[w].Add(label);
                        }
                    }
                }
            }
        }

        // exact method to call for query
        public HashSet<string> GetBroaderConcepts(string query,
            bool oneCandidate = false, bool shared = false, double minSim=0.5,
            bool fullQueryComparision = true)
        {
            HashSet<string> candidates = GetProcessedCandidates(query, oneCandidate,
                    minSim, fullQueryComparision);
            return GetAllOrSharedBroaders(candidates, shared);
        }

        // exact method to call for query
        public HashSet<string> GetNarrowerConcepts(string query,
            bool oneCandidate = false, bool shared = false, double minSim=0.5,
            bool fullQueryComparision = true)
        {
            HashSet<string> candidates = GetProcessedCandidates(query, oneCandidate,
                    minSim, fullQueryComparision);
            return GetAllOrSharedNarrowers(candidates, shared);
        }

        // exact method to call for query
        public HashSet<string> GetRelatedConcepts(string query,
            bool oneCandidate = false, bool shared = false, double minSim=0.5,
            bool fullQueryComparision = true)
        {
            HashSet<string> candidates = GetProcessedCandidates(query, oneCandidate,
                    minSim, fullQueryComparision);
            HashSet<string> broaders = GetBroaderConcepts(query, oneCandidate,
                    shared, minSim, fullQueryComparision);
            var all_related = GetAllOrSharedNarrowers(broaders, shared);
            all_related.ExceptWith(candidates);
            return all_related;
        }

        public HashSet<string> GetProcessedCandidates(string query, bool oneCandidate = false,
            double minSim=0.5, bool fullQueryComparision = true)
        {
            HashSet<string> candidates = null;
            if (oneCandidate)
            {
                candidates = new HashSet<string>();
                var candidate = GetCandidate(query, minSim: minSim,
                        fullQueryComparision: fullQueryComparision);
                if (candidate == null)
                {
                    return EMPTY_SET;
                }
                candidates.Add(candidate);
            } else {
                candidates = GetCandidates(query, minSim: minSim,
                        fullQueryComparision: fullQueryComparision);
            }
            Console.WriteLine(String.Format("  Candidates for '{0}': '{1}'", query, string.Join(" | ", candidates)));
            return candidates;
        }

        // get candidate concepts from graph matching words in query
        public HashSet<string> GetCandidates(string query, double minSim=0.5,
            bool fullQueryComparision = true)
        {
            var candidates = new HashSet<string>();
            query = query.ToLower();
            var words = Regex.Split(query, @"\s");
            foreach (string w in words)
            {
                // check if there is any word matching in index
                if (WordLabels.ContainsKey(w))
                {
                    foreach (var c in WordLabels[w])
                    {
                        if (c != null)
                        {
                            double sim = 0.0;
                            if (fullQueryComparision)
                            {
                                sim = LevSim.LevenshteinSimilarity(query, c);
                                Console.WriteLine(String.Format("Sim. between query '{0}' and '{1}': {2}",
                                    query, c, sim));
                            } else {  // comparision with single matching word
                                    // TODO any better idea?
                                sim = LevSim.LevenshteinSimilarity(w, c);
                                Console.WriteLine(String.Format("Sim. between '{0}' and '{1}': {2}",
                                    query, c, sim));
                            }
                            if (sim >= minSim)
                            {
                                candidates.Add(c);
                                Console.WriteLine(String.Format("Adding candidate '{0}'", c));
                            }
                        }
                    }
                }
            }
            return candidates;
        }

        // similar as GetCandidates but returns only one candidate with highest
        // similarity score
        public string GetCandidate(string query, double minSim=0.5,
            bool fullQueryComparision = true)
        {
            string candidate = null;
            double maxSim = 0.0;
            query = query.ToLower();
            var words = Regex.Split(query, @"\s");
            foreach (var w in words)
            {
                // check if there is any word matching in index
                if (WordLabels.ContainsKey(w))
                {
                    foreach (var c in WordLabels[w])
                    {
                        double sim = 0.0;
                        if (fullQueryComparision)
                        {
                            sim = LevSim.LevenshteinSimilarity(query, c);
                        } else {  // comparision with single matching word
                                  // TODO any better idea?
                            sim = LevSim.LevenshteinSimilarity(w, c);
                        }
                        if (sim >= minSim)
                        {
                            if (sim > maxSim)
                            {
                                candidate = c;
                                maxSim = sim;
                            }
                        }
                    }
                }
            }
            return candidate;
        }


        public HashSet<String> GetBroaders(string vlabel)
        {
            return Broaders.ContainsKey(vlabel) ? Broaders[vlabel] : new HashSet<string>();
        }

        public HashSet<string> GetAllBroaders(HashSet<string> vlabels)
        {
            var res = new HashSet<string>();
            foreach (var l in vlabels)
            {
                res.UnionWith(GetBroaders(l));
            }
            return res;
        }

        public HashSet<string> GetAllOrSharedBroaders(HashSet<string> vlabels, bool shared)
        {
            if (shared)
            {
                return GetSharedBroaders(vlabels);
            }
            return GetAllBroaders(vlabels);
        }

        public HashSet<string> GetAllOrSharedNarrowers(HashSet<string> vlabels, bool shared)
        {
            if (shared)
            {
                return GetSharedNarrowers(vlabels);
            }
            return GetAllNarrowers(vlabels);
        }


        public HashSet<string> GetSharedBroaders(HashSet<string> vlabels)
        {
            var res = GetAllBroaders(vlabels);
            foreach (var l in vlabels)
            {
                res.IntersectWith(GetBroaders(l));
            }
            return res;
        }

        public HashSet<string> GetAllNarrowers(HashSet<string> vlabels)
        {
            var res = new HashSet<string>();
            foreach (var l in vlabels)
            {
                res.UnionWith(GetNarrowers(l));
            }
            return res;
        }

        public HashSet<string> GetSharedNarrowers(HashSet<string> vlabels)
        {
            var res = GetAllNarrowers(vlabels);
            foreach (var l in vlabels)
            {
                res.IntersectWith(GetNarrowers(l));
            }
            return res;
        }

        public HashSet<String> GetNarrowers(string vlabel)
        {
            return Narrowers.ContainsKey(vlabel) ? Narrowers[vlabel] : new HashSet<string>();
        }

        public void PrintBroaders(string vlabel)
        {
            var broaders = GetBroaders(vlabel);
            Console.WriteLine("Broaders for " + vlabel + ": " + string.Join(" | ", broaders));
        }

        public void PrintNarrowers(string vlabel)
        {
            var narrowers = GetNarrowers(vlabel);
            Console.WriteLine("Narrowers for " + vlabel + ": " + string.Join(" | ", narrowers));
        }

        public void PrintLabels()
        {
            foreach (var node in this.Graph.Nodes())
            {
                string label = "";
                bool hasLabel = (LabelsMap != null && LabelsMap.TryGetValue(node, out label));
                Console.WriteLine("Node "+node+": label is "+(hasLabel ? label.ToString() : node.ToString()));
            }

        }


        // quick solution to include some tests
        public static void ManualTests()
        {
            var graphPath = @"computer-science.graphml";
            // var nodeIdPropName = "lod_url";
            var nodeIdPropName = "label";
            var edgePropName = "label";
            var g = new SemanticGraph(graphPath, nodeIdPropName, edgePropName);

            Console.WriteLine("Processing graph done!");
            // g.PrintLabels();
            // var br = g.Broaders["http://dbpedia.org/resource/Category:Philosophers_of_art"];
            var br = g.Broaders.Keys;
            // g.PrintBroaders("http://dbpedia.org/resource/Category:Harrogate");
            // g.PrintBroaders("http://dbpedia.org/resource/Category:Education_in_Río_Negro_Province");
            // g.PrintNarrowers("http://dbpedia.org/resource/Category:Harrogate");
            g.PrintNarrowers("Informatyka");
            g.PrintBroaders("Informatyka");
            g.PrintNarrowers("Bioinformatyka");
            g.PrintBroaders("Bioinformatyka");
            g.PrintBroaders("Dane");
            Console.WriteLine(string.Join(" | ", g.GetAllBroaders(new HashSet<String> { "Dane", "Bioinformatyka" })));
            Console.WriteLine(string.Join(" | ", g.GetSharedBroaders(new HashSet<String> { "Dane", "Bioinformatyka" })));
            Console.WriteLine(string.Join(" | ", g.GetAllBroaders(new HashSet<String> { "Dane", "Oprogramowanie" })));
            Console.WriteLine(string.Join(" | ", g.GetSharedBroaders(new HashSet<String> { "Dane", "Oprogramowanie" })));
            var lev = new Levenshtein();
            Console.WriteLine(lev.LevenshteinSimilarity("Dane", "Oprogramowanie"));
            Console.WriteLine(lev.LevenshteinSimilarity("Dane", "Oprogramowanie"));
            Console.WriteLine(lev.LevenshteinSimilarity("Oprogramowanie bioinformatyczne", "Oprogramowanie"));
            Console.WriteLine(lev.LevenshteinSimilarity("Oprogramowanie informatyczne", "Oprogramowanie"));
            Console.WriteLine(lev.LevenshteinSimilarity("Oprogramowanie", "Oprogramowanie"));
            Console.WriteLine();
            Console.WriteLine(string.Join(" | ", g.WordLabels["dane"]));
            Console.WriteLine(string.Join(" | ", g.WordLabels["oprogramowanie"]));
            Console.WriteLine("Cand. for 'Oprogramowanie': " + string.Join(" | ", g.GetCandidates("Oprogramowanie")));
            Console.WriteLine("Broader for 'Oprogramowanie': "+string.Join(" | ", g.GetBroaderConcepts("Oprogramowanie")));
            Console.WriteLine("Nr. for 'Oprogramowanie': "+string.Join(" | ", g.GetNarrowerConcepts("Oprogramowanie")));
            Console.WriteLine("Br. for 'Program': "+string.Join(" | ", g.GetBroaderConcepts("Program")));
            Console.WriteLine("Br. for 'Dane': "+string.Join(" | ", g.GetBroaderConcepts("Dane")));
            Console.WriteLine("Nr. for 'Program': "+string.Join(" | ", g.GetNarrowerConcepts("Program")));
            Console.WriteLine("Br. for 'Program Dane': "+string.Join(" | ", g.GetBroaderConcepts("Program Dane", minSim: 0.2)));
            Console.WriteLine("Nr. for 'Program dane': "+string.Join(" | ", g.GetNarrowerConcepts("Program Dane", minSim: 0.2)));
            Console.WriteLine("Nr. for 'Algorytm Dane': "+string.Join(" | ", g.GetNarrowerConcepts("Algorytm Dane")));
            Console.WriteLine("Related for 'Dane': "+string.Join(" | ", g.GetRelatedConcepts("Dane")));

        }
    }
}
