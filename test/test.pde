import mqtt.*;

MQTTClient client;

void setup() {
  client = new MQTTClient(this);
  client.connect("mqtt://0.0.0.0:1884", "processing");
  
  frameRate(1);
}

void draw() {
  String v1 = Float.toString(random(1000));
  String v2 = Float.toString(random(1000));
  String v3 = Float.toString(random(1000));
  
  client.publish("value", v1+","+v2+","+v3);
}

void messageReceived(String topic, byte[] payload) {
  println("new message: " + topic + " - " + new String(payload));
}