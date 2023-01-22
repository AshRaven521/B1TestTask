import { useEffect, useState } from "react";
import { FileUploader } from "react-drag-drop-files";

import { getFiles, loadFile, getFile } from "./api";
import styles from "./styles.module.css";
import { Table } from "./table";

export function App() {
  const [files, setFiles] = useState([]);
  const [uploadedFile, setUploadedFile] = useState(null);
  const [selectedFile, setSelectedFile] = useState(null);

  const uploadFile = async () => {
    if (uploadedFile) {
      const newFile = await loadFile(uploadedFile);
      setFiles((files) => [...files, newFile]);
    }
  };

  useEffect(() => {
    getFiles().then(setFiles);
  }, []);

  const getFileContent = async (id) => {
    const file = await getFile(id);
    setSelectedFile(file);
  };

  return (
    <div className={styles.container}>
      <div className={styles.tools}>
        <div className={styles.dnd_zone}>
          <FileUploader handleChange={setUploadedFile} />
          <button onClick={uploadFile}>Upload</button>
        </div>
        {!!files.length && (
          <div className={styles.files}>
            {files.map(({ id, fileName }) => (
              <span className={styles.file} key={id} onClick={getFileContent.bind(null, id)}>
                {fileName}
              </span>
            ))}
          </div>
        )}
      </div>
      {selectedFile && <Table file={selectedFile} />}
    </div>
  );
}
