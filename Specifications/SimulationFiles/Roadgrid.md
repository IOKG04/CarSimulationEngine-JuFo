**Roadgrids are saved as a list of points (-> Crossings and corners) and a list of point lists (-> Roads)**

Int64	Number of points
\[
  Double	Position X
  Double	Position Y
\]
Int64	Number of roads
\[
  Double  Speed limit
  Int64 Lanes
  Int64	Number of points
  \[
    Int64	Index of point
    Bool	Is corner
  \]
\]
