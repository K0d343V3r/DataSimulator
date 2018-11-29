# DataSimulator.Api
Data simulation services for industrial automation applications.  

##### Features:

- 3 types of data streams: numerical, boolean, and string.
- Common data patterns for numerical streams:  sine, square, triangle, sawtooth waveforms, white noise (random), and incremental count.
- 2 types of boolean streams:  periodic (constant cadence) pulse and modulated (variable cadence) pulse.
- String stream returns date and time string, in ISO 8601 format.
- Supports historical (time series), live (current), and point in time data retrieval.
- Supports historical retrieval via absolute time requests (i.e. from X, to Y) and relative time requests (i.e X minutes before now, to now).
- Supports filtering of large data streams via sample and hold interpolation.

##### Implementation:

- ASP.NET Core Web API 2.1.
- Swagger document support.
- CORS support.




