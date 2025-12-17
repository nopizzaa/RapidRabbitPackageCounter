import express from 'express';
import { join } from 'path';

const app = express();
const port = process.env.PORT || 4000;

// Angular build könyvtár
const distFolder = join(__dirname, 'dist/browser');
const indexHtml = 'index.html';

// Statikus fájlok kiszolgálása
app.use(express.static(distFolder, {
  maxAge: '1y',
}));

// Minden más request az Angular index.html-re
app.get('*', (req, res) => {
  res.sendFile(join(distFolder, indexHtml));
});

// Szerver indítása
app.listen(port, () => {
  console.log(`Server running at http://localhost:${port}`);
});
