export const convertFileToBinary = (file) => {
  const blob = new Blob([file], { type: file.type });
  const formData = new FormData();
  formData.append("fileDetails", blob, file.name);
  return formData;
};
