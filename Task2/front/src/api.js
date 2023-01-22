import axios from "axios";

import { convertFileToBinary } from "./helpers";

axios.defaults.baseURL = "https://localhost:5001/api";

export const loadFile = (file) => {
  const fileDetails = convertFileToBinary(file);
  return axios
    .post("files/single-file", fileDetails, {
      headers: {
        "content-type": "multipart/form-data",
      },
    })
    .then(({ data }) => data);
};

export const getFiles = () => {
  return axios.get("files/files-names").then(({ data }) => data);
};

export const getFile = async (fileId) => {
  const { data } = await axios.get("files/file-by-id", { params: { fileId } });
  return data;
};
