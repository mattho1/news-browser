using Backend.DTOs;
using Backend.Helpers;
using Backend.Models;
using Backend.Models.Semantic;
using Backend.Repositories.Abstract;
using Backend.Services.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Backend.Services.Concrete
{
    public class SemanticService: ISemanticService
    {
        private SemanticGraph Graph;

        // private string DefaultGraphPath = @"computer-science.graphml";
        private string DefaultGraphPath = @"dbpedia-cat-graph-broader-only-top-3.graphml";
        // private string DefaultGraphPath = @"dbpedia-graph-categories-only-broader-v2.graphml";

        private string DefaultNodeIdPropName = "label";

        private string DefaultEdgePropName = "label";

        private bool OneCandidate = false;
        private bool Shared = false;
        private double MinSim = 0.5;
        private bool FullQueryComparision = true;

        /// Parameters:
        /// graphPath - path to *.graphml graph. Such graph must contains
        ///             'broader' and 'narrower' relations
        /// nodeIdPropName - name of vertex property storing label of concept
        /// edgePropName - name of edge property storing label or relationship
        ///                type
        /// minSim - minimal score of similarity (currently Levenshtein) for word
        ///          from query and label of concept, allowing matching such
        ///          concept to this word
        /// oneCandidate - if enabled, then only one candidate from graph will
        ///                be matched to corresponding word from query; otherwise,
        ///                all candidates above similarity score
        /// shared - if enabled, then in case of many matching concepts,
        ///          intersection will be used; otherwise - union
        /// fullQueryComparision - decide whether compare full query content
        /// with concepts (when matching) or each word from query separately
        // public SemanticService(string graphPath = null, string nodeIdPropName = null,
        //     string edgePropName = null, bool oneCandidate = false,
        //     bool shared = false, double minSim=0.5,
        //     bool fullQueryComparision = true)
        // {
        //     var selGraphPath = graphPath != null ? graphPath : DefaultGraphPath;
        //     var selNodeIdPropName = nodeIdPropName != null ? nodeIdPropName : DefaultNodeIdPropName;
        //     var selEdgePropName = edgePropName != null ? edgePropName : DefaultEdgePropName;

        //     Graph = new SemanticGraph(selGraphPath, selNodeIdPropName, selEdgePropName);
        // }

        public SemanticService()
        {
            Graph = new SemanticGraph(DefaultGraphPath, DefaultNodeIdPropName,
                DefaultEdgePropName);
        }

        public IEnumerable<string> GetBroaderConcepts(string searchQuery)
        {
            return Graph.GetBroaderConcepts(searchQuery, OneCandidate, Shared, MinSim,
                    FullQueryComparision);
        }

        public IEnumerable<string> GetNarrowerConcepts(string searchQuery)
        {
            return Graph.GetNarrowerConcepts(searchQuery, OneCandidate, Shared, MinSim,
                    FullQueryComparision);
        }

        public IEnumerable<string> GetRelatedConcepts(string searchQuery)
        {
            return Graph.GetRelatedConcepts(searchQuery, OneCandidate, Shared, MinSim,
                    FullQueryComparision);
        }
    }

    // public class SemanticService: ISemanticService
    // {

    //     public SemanticService()
    //     {

    //     }

    //     public IEnumerable<string> GetBroaderConcepts(string searchQuery)
    //     {
    //         return new List<string> {"a", "b"};
    //     }

    //     public IEnumerable<string> GetNarrowerConcepts(string searchQuery)
    //     {
    //         return new List<string> {"a", "d"};
    //     }

    //     public IEnumerable<string> GetRelatedConcepts(string searchQuery)
    //     {
    //         return new List<string> {"e", "b"};
    //     }
    // }
}
