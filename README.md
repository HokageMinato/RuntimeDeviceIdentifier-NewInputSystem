# RuntimeDeviceIdentifier-NewInputSystem
A Runtime device identifier which subscribes to OnInputAction and tries to identify devices based on defined classification. With cached approach the resolve is instantaneous and doesnt affect frame time even a bit compared to standard unity approach.

Benchmarks:
![Benchmark time in ms for device type resolve over 2000 iterations]( https://i.ibb.co/N2dsLG9/Device-Hardware-Identifier.png )
