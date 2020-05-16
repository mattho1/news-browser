import { Tag } from "./tag";

export interface SimpleNews {
  id: string;
  url: string;
  title: string;
  author: string;
  text: string;
  tags: Tag[];
}

export const EMPTY_SIMPLE_NEWS = {
  id: "",
  url: "",
  title: "",
  author: "",
  text: "",
  tags: []
}
