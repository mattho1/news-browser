export interface NewsDetails {
  id: string;
  url: string;
  title: string;
  author: string;
  text: string;
  tags: string[];
  imageUrl: string;
  language: string;
  siteSection: string;
  titleSection: string;
}

export const EMPTY_NEWS_DETAILS = {
  id: "",
  url: "",
  title: "",
  author: "",
  text: "",
  tags: [],
  imageUrl: "",
  language: "",
  siteSection: "",
  titleSection: "",
}
