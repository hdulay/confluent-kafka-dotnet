# Confluent Cloud Producer

This example produces to confluent cloud for 15 minutes.

## Build the project

From the root of this repository, build the project.

```cmd
make build
```

### Create a script to write data to Confluent Cloud

```bash
#!/bin/bash
dotnet run bootstrap_servers key secret your-topic
```

## Create a script to read the data from Confluent Cloud

```bash
#!/bin/bash
ccloud login
ccloud environment use env-XXXXX
ccloud kafka cluster use lck-XXXXXX
ccloud kafka topic consume --api-key key --api-secret secret your-topic
```

## sample output

output is format

```text
RANDOM_TEXT message_count elapsed[time]

```text
...
MEGOA 42233490 elapsed[00:14:59.9995133]
FWCPB 42233498 elapsed[00:14:59.9995538]
BAWQD 42233499 elapsed[00:14:59.9995596]
WWUQK 42233501 elapsed[00:14:59.9995704]
WNBTL 42233509 elapsed[00:14:59.9996117]
YLWWV 42233516 elapsed[00:14:59.9996470]
JRNQU 42233520 elapsed[00:14:59.9996675]
VMORG 42233535 elapsed[00:14:59.9997439]
HOXHS 42233537 elapsed[00:14:59.9997531]
DCFGN 42233540 elapsed[00:14:59.9997681]
FGAQO 42233542 elapsed[00:14:59.9998033]
JTBHG 42233543 elapsed[00:14:59.9998087]
HBMYV 42233561 elapsed[00:14:59.9999006]
QUIUV 42233569 elapsed[00:14:59.9999403]
```