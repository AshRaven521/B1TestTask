const config = [
  { label: "Б/сч", name: "countId" },
  { label: "Входящее активное сальдо", name: "inputActive" },
  { label: "Входящее пассивное сальдо", name: "inputPassive" },
  { label: "Дебет", name: "debit" },
  { label: "Кредит", name: "credit" },
  { label: "Исходящее активное сальдо", name: "outputActive" },
  { label: "Исходящее пассивное сальдо", name: "outputPassive" },
];

export function Table({ file }) {
  return (
    <table>
      <thead>
        <tr>
          {config.map(({ label }, i) => (
            <td key={i}>{label}</td>
          ))}
        </tr>
      </thead>
      <tbody>
        {file.map((row, i) => (
          <tr key={i}>
            {config.map(({ name }, j) => (
              <td key={j}>{row[name]}</td>
            ))}
          </tr>
        ))}
      </tbody>
    </table>
  );
}
