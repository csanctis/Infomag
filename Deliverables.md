# Azure IoT Solution Deliverables

## 1. Scalable Azure Architecture

### Architecture Overview

To be able to process 10k assets sending data every few seconds (~50k messages/minute), I decided to go with the following multi-tier architecture:
This IOT project was my first interaction with this kind of fast processing data, so I researched a little and found that these services would be the best approach.
My initial thought that Functions would work as the first entry point would not work.


```
IoT Hub → Stream Analytics → ┌─→ SignalR (Real-time)
                            ├─→ Cosmos DB (Hot storage)
                            ├─→ Event Hub → Data Factory → Synapse (Analytics)
                            └─→ Data Lake (Cold storage)
```



**Ingestion Layer:**
- **Azure IoT Hub** - Primary ingestion point with device management. The IOT Hub will communicate to/from the devices and process all the messages quickly.

**Processing Layer:**
- **Azure Stream Analytics** - Real-time processing and routing. This is a very capable service that will allow us to write the data to multiple services, if required.
It allows us to setup direct data insertion into CosmosDB, passing the data to functions, and even processing of the data before storage

**Storage Layer:**
- **Azure Cosmos DB** - Hot path storage for real-time queries (my initial suggestion would be to have a partition by deviceId, to help with queries/data limit.)

**API Layer:**
- I believe we could deploy the API using either **Azure API Management** or **Azure App Service**, with both providing Rate limiting, caching, and security Auto-scaling web APIs with SignalR
- For real-time communication with the clients, I decided to use **Azure SignalR Service**, which is a very robust and tested framework

### Scalability Strategies:

We suggest the following approaches for scalability

1. **Horizontal Partitioning** - Partition data by device ID and timestamp
2. **Auto-scaling** - Configure scaling rules based on message throughput
3. **Load Balancing** - Distribute across multiple regions if needed
4. **Caching** - Redis cache for frequently accessed device states
5. **Batch Processing** - Aggregate non-critical data in batches to reduce costs

## 2. Real-time Dashboard

There are 2 main data storage strategies here. Hot and Cold Patch.

- Hot path would involve any real-time data that is required in an instant.
- Cold Patch would be the archival data, that may take some time to be recovered, without impact

My current solution only uses CosmosDB, but further improvements can be made.
The following are the suggestion for both:

**Hot Path (Real-time Dashboard):**
- **Azure Stream Analytics** - Process incoming data with <5 second latency
- **Azure SignalR Service** - Push live updates to dashboard clients
- **Azure Cosmos DB** - Store last 24 hours of data for instant queries
- **Real-time aggregations** - Calculate rolling averages, alerts, device status
- **Azure Cache for Redis** - Cache current device states and KPIs

**Cold Path (Long-term Analytics):**
- **Azure Data Factory** - Orchestrate ETL pipelines for historical data
- **Azure Synapse Analytics** - Data warehouse for complex analytical queries
- **Azure Data Lake Storage Gen2** - Partitioned by year/month/day for cost optimization
- **Delta Lake format** - Enable ACID transactions and time travel queries

**Performance Optimizations:**
- **Data tiering** - Hot (24h), Warm (30d), Cold (1y+)

## 3. Automated Monitoring & Anomaly Detection

For monitoring and failures detection, we can use Azure Monitor, AppInsights and other metrics provided by Azure.

- **Azure Monitor** - Centralized metrics and logs collection
- **Application Insights** - API performance, dependency tracking, custom telemetry
- **Log Analytics Workspace** - Centralized logging with KQL queries
- **Azure Service Health** - Service availability and planned maintenance alerts

Suggestion of Metrics to be monitored

- **IoT Hub**: Message throughput, device connectivity, throttling
- **Stream Analytics**: Processing latency, failed events, resource utilization
- **Cosmos DB**: RU consumption, partition hot spots, query performance
- **SignalR**: Connection count, message delivery rate

### Anomaly Detection Implementation:

**Real-time Anomaly Detection:**
- **Azure Anomaly Detector** - ML-based detection on telemetry patterns
- **Stream Analytics** - Real-time rules for threshold violations
- **System health** - Resource utilization thresholds

### Automated Response Actions:

For real time notifications, we can implement alerts to be sent to the team, according to the priority.
So we could send SMS, emails, dashboard notifications, for example

### Cost Control Measures:

4. **Resource tagging** - Track costs by component and environment
1. **Log retention policies** - Archive old data (30d hot, 90d warm, 1y archive)
2. **Metric aggregation** - Pre-aggregate high-frequency data
3. **Alert throttling** - Prevent alert storms with cooldown periods

## 4. Troubleshooting & Security Incident Response

In conjunction with the steps described in step 3, this are my recommended steps to verify an incident

1. **Device Level Diagnostics:**
   ```
   Check: Device logs, network connectivity, certificate validity
   Tools: IoT Hub device diagnostics, connection state monitoring
   ```

2. **IoT Hub Ingestion:**
   ```
   Metrics: Message count, throttling events, authentication failures
   Logs: Device-to-cloud telemetry, connection events filtering by specific device ID and timestamp range
   ```

3. **Stream Analytics Processing:**
   ```
   Check: Input/output events, processing errors, query logic
   Validate: Data transformation rules, filtering conditions
   ```

4. **Storage Layer Verification:**
   ```
   Database: Query directly for device data, check partition health
   ```

### Security Considerations

These are the standard recommendations for security, according to various different websites

**Authentication & Authorization:**
- **Device certificates** - Verify X.509 certificates haven't expired
- **SAS tokens** - Check token validity and permissions
- **API authentication** - Validate JWT tokens and claims
- **Network security** - Ensure TLS encryption end-to-end

**Potential Security Issues:**
1. **Man-in-the-middle attacks** - Certificate pinning validation
2. **Token hijacking** - Check for unusual access patterns
3. **Data tampering** - Verify message integrity and signatures
4. **DDoS attacks** - Monitor for unusual traffic patterns

## 5. Azure Service Selection

How do you help developers understand why you chose a certain Azure service over another?

Whenever a decision is made, I like to bring the team together, explain the problem, the alternatives being considered to solve the problem, and the reasoning for a selection.
Then you can ask the team if anyone has any concerns/questions. Most of the time, if your decision is good enough, the team will accept it without any problems.
But it is good to raise this, because someone may have already faced this before and may see the problem from another perspective.
At the end, the team needs to own the solution and feel they were part of the decision making proccess. 
In my opinion, this is the best way to bring the team together, accepting one solution and commiting to it.

## 6. Managing Chaos

These are the strategies I've previously used to manage quick fixes/patches to Production

**Pre-Deployment Checklist:**

1. **Code Quality Gates:**
   ```
   ✓ Unit tests in place (with minimum coverage)
   ✓ Static code analysis (SonarQube/CodeQL)
   ✓ QA 
   ```

2. **Configuration Management:**
   ```
   ✓ Deployment via pipelines (automated)
   ✓ Feature flags for gradual rollout
   ✓ Rollback plan documented
   ```

3. **Environment Isolation:**
   ```
   ✓ Use separate environment for testing (QA/UAT)
   ```

### Risk Mitigation

Having a rollback strategy in place can really save you in a hard time. Here are some options: 

```
Automatic: Health check failures → Auto-rollback in 5 minutes
Manual: rollback via deployment pipelines
Data: Database migration rollback scripts ready
```

### Communication Protocol

This varies based on the company/project/customer base you have, but in general it is good to have a communication protocol, in place, wo everyone is aware of when something new is being deployed.

**Stakeholder Updates:**
- **Customer** - Deployment window notification with rollback plan
- **Team** - Dedicated Slack channel for deployment status
- **Management** - Go/No-go decision points with clear criteria

**Documentation**
- Deployment steps and validation checklist
- Known issues and workarounds
- Emergency contact information
- Post-deployment verification steps

### Post-Deployment Validation

**Immediate Checks:**
1. Services are executing as expected
3. No critical errors in logs
4. Downstream dependencies responding normally

**Extended Monitoring:**
1. Performance under normal load
2. Memory and CPU utilization trends
3. Customer feedback and support tickets
4. Business metrics validation

### Emergency Procedures

**If Issues Detected:**
```
Minor Issues: Monitor and document
Major Issues: Immediate rollback + incident response
Critical Issues: Rollback + customer communication + post-mortem
```

This last one I've been doing in my current job and it is a really good tool to help the team to improve and prevent future problems
**Lessons Learned:**
- Schedule post-deployment retrospective within 24 hours
- Update deployment checklist based on findings
- Improve automation to reduce manual steps