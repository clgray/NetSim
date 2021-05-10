{
  "InfluxDBConfig": {
    "Hostname": "",
    "Database": "",
    "User": "",
    "Password": ""
  },
  "NetworkSettings": {
    "TimeDelta": 1,
    "NodeSettings": [
      {
        "Id": "1",
        "RoutingAlgorithm": "dijkstra",
        "Throughput": 10
      }
    ],
    "ConnectionSettings": [
      {
        "Bandwidth": 10,
        "TimeUntilShutdown": 0,
        "NodeIds": ["1", "2"]
      }
    ],
    "MessagesSettings": {
      "Quantity": 10,
      "Size": 1,
      "SizeRange": 0,
      "Seed": 1000
    }
  }
}