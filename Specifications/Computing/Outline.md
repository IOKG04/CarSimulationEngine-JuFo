| ***Car*** |	  | ***API*** |   | ***Simulation Engine*** |   | ***Other APIs*** |   | ***Other Cars*** |
| --- | --- | --- | --- | --- | --- | --- | --- | --- |
| Joines network | --Simulation request-> | Formats request |   |   |   |   |   | Driving |
| Waiting |   | Sends formated request | --Formated request-> | Simulating |   |   |   | Driving |
| Waiting |   | Waiting |   | Simulating |   |   |   | Driving |
| Waiting |   | Formats path | \<-Path-- | Sends path(s) | *--Path (if needed)->* | Formats path |   | Driving |
| Starts driving | \<-Formated path-- | Sends formated request |   |   |   | Sends formated path | --Formated path-> | Adjusts driving |
