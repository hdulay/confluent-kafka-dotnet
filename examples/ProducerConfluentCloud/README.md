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
