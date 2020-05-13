export interface SimpleNews {
  id: string;
  url: string;
  title: string;
  author: string;
  text: string;
  tags: string[];
}

export const EMPTY_SIMPLE_NEWS = {
  id: "",
  url: "",
  title: "",
  author: "",
  text: "",
  tags: []
}
