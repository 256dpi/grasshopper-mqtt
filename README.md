# grasshopper-mqtt

**A grasshopper component that connects to a MQTT broker.**

![Screenshot](http://joel-github-static.s3.amazonaws.com/grasshopper-mqtt/screenshot.png)

1. Download the latest component from [releases](https://github.com/256dpi/grasshopper-mqtt/releases).
2. Open Rhino and Grasshopper.
3. Put the component on the canvas and provde a text input with the brokers URI. The URI needs to be constructed as follows: `mqtt://[username[:password]@]host[:port]/[topic]` (stuff in square brackets is optional). If topic is provided the client subscribe to all messages.
4. The component will connect to the broker and cache incomming messages.
5. Attach a timer to the component to trigger an update of the outputs.

## Developing

1. Install RhinoWIP & Visual Studio Community for Mac
2. Install https://github.com/mcneel/RhinoCommonXamarinStudioAddin
