# apc2mqtt

[![Software
License](https://img.shields.io/badge/License-MIT-orange.svg?style=flat-square)](https://github.com/mannkind/apc2mqtt/blob/main/LICENSE.md)
[![Build Status](https://github.com/mannkind/apc2mqtt/workflows/Main%20Workflow/badge.svg)](https://github.com/mannkind/apc2mqtt/actions)
[![Coverage Status](https://img.shields.io/codecov/c/github/mannkind/apc2mqtt/main.svg)](http://codecov.io/github/mannkind/apc2mqtt?branch=main)

An experiment to publish APC UPS metrics to MQTT.

## Use

The application can be locally built using `dotnet build` or you can utilize the multi-architecture Docker image(s).

### Example

```bash
docker run \
-e APC__RESOURCES__0__HOST="localhost" \
-e APC__RESOURCES__0__PORT="3551" \
-e APC__RESOURCES__0__SerialNo="ABCDEFG" \
-e APC__RESOURCES__0__Slug="home" \
-e APC__MQTT__BROKER="localhost" \
-e APC__MQTT__DISCOVERYENABLED="true" \
mannkind/apc2mqtt:latest
```

OR

```bash
APC__RESOURCES__0__HOST="localhost" \
APC__RESOURCES__0__PORT="3551" \
APC__RESOURCES__0__SerialNo="ABCDEFG" \
APC__RESOURCES__0__Slug="home" \
APC__MQTT__BROKER="localhost" \
APC__MQTT__DISCOVERYENABLED="true" \
./apc2mqtt 
```


## Configuration

Configuration happens via environmental variables

```bash
APC__RESOURCES__#__SerialNo                 - The serial no. for a specific APC UPS
APC__RESOURCES__#__Host                     - [OPTIONAL] The apcupsd host for a specific APC UPS, defaults to "localhost"
APC__RESOURCES__#__Port                     - [OPTIONAL] The apcupsd port for a specific APC UPS, defaults to "3551"
APC__RESOURCES__#__Slug                     - The slug to identify the specific host
APC__POLLINGINTERVAL                    - [OPTIONAL] The delay between lookups, defaults to "0.00:01:07"
APC__MQTT__TOPICPREFIX                  - [OPTIONAL] The MQTT topic on which to publish the collection lookup results, defaults to "home/apc"
APC__MQTT__DISCOVERYENABLED             - [OPTIONAL] The MQTT discovery flag for Home Assistant, defaults to false
APC__MQTT__DISCOVERYPREFIX              - [OPTIONAL] The MQTT discovery prefix for Home Assistant, defaults to "homeassistant"
APC__MQTT__DISCOVERYNAME                - [OPTIONAL] The MQTT discovery name for Home Assistant, defaults to "apc"
APC__MQTT__BROKER                       - [OPTIONAL] The MQTT broker, defaults to "test.mosquitto.org"
APC__MQTT__USERNAME                     - [OPTIONAL] The MQTT username, default to ""
APC__MQTT__PASSWORD                     - [OPTIONAL] The MQTT password, default to ""
```
