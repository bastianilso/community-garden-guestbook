#include <ESP8266WiFi.h>

//////////////////////
// WiFi Definitions //
//////////////////////
const char WiFiAPPSK[] = "GPSOUTH-78";

/////////////////////
// Pin Definitions //
/////////////////////
const int LED_PIN = 2; // Thing's onboard, green LED
const int ANALOG_PIN = A0; // The only analog pin on the Thing
const int PING_PIN = 13; // Digital pin to be read

long upperBound = 10000;
long lowerBound = 1000;
bool visitor = false;
String s = "";
int countDown = 3;

WiFiServer server(80);

void setup() {
  // put your setup code here, to run once:
  initHardware();
  setupWiFi();
  server.begin();
}

void loop() {
  // put your main code here, to run repeatedly:

  long duration, distance;

  pinMode(PING_PIN, OUTPUT);
  
  // The PING))) is triggered by a HIGH pulse of 2 or more microseconds.
  // Give a short LOW pulse beforehand to ensure a clean HIGH pulse:
  digitalWrite(PING_PIN, LOW);
  delayMicroseconds(2);
  digitalWrite(PING_PIN, HIGH);
  delayMicroseconds(5);
  digitalWrite(PING_PIN, LOW);

  pinMode(PING_PIN, INPUT);
  duration = pulseIn(PING_PIN, HIGH);
  Serial.println(duration);
  delay(50);

    if (duration > lowerBound && duration < upperBound) {
      Serial.println("DETECTED");
      //Serial.println(visitor);
      //Serial.println(countDown);
      countDown -= 1;
      if (!visitor && countDown < 0) {
        Serial.println("VISITOR");
        s = "Content-Type: text/html\r\n\r\n";
        s += duration;
        s += ',';
        s += millis()/1000;
        visitor = true;
        countDown = 3;
        digitalWrite(LED_PIN, 0);
      }
    } else {
      visitor = false;
      if (countDown < 3) {
        countDown += 1;
      }
      digitalWrite(LED_PIN, 1);
      
    }

  // Check if a client has connected
  WiFiClient client = server.available();
  if (!client) {
    return;
  }
  // Read the first line of the request
  String req = client.readStringUntil('\r');
  Serial.println(req);
  client.flush();

  // Match the request
  // int val = -1; // We'll use 'val' to keep track of both the
                // request type (read/set) and value if set.
  //if (req.indexOf("/led/on") != -1)
  //  digitalWrite(LED_PIN, 0); // Will turn LED on
  //else if (req.indexOf("/led/off") != -1)
  //  digitalWrite(LED_PIN, 1); // Will turn LED off
  //else if (req.indexOf("/distance") != -1)
  //  val = 2; // Will turn LED off
  //else if (req.indexOf("/read") != -1)
  //  val = -2; // Will print pin reads
  // Otherwise request will be invalid. We'll say as much in HTML

  //client.flush();

  // Prepare the response. Start with the common header:
  //String s = "HTTP/1.1 200 OK\r\n";
  //s += "<!DOCTYPE HTML>\r\n<html>\r\n";

  // If we're setting the LED, print out a message saying we did
  //if (val >= 0)
  //{
  //  s += "LED is now ";
  //  s += (val)?"off":"on";
  //}
  //else if (val == -2)
  //{ // If we're reading pins, print out those values:
    //s += "Distance = ";

    /* The following trigPin/echoPin cycle is used to determine the
    distance of the nearest object by bouncing soundwaves off of it. */

    //Calculate the distance (in cm) based on the speed of sound.
    //distance = duration / 29 / 2;
    //s += distance;
    
    
    //Delay 50ms before next reading.
    delay(1000);


    //s += "<br>"; // Go to the next line.
    //s += "Analog Pin = ";
    //s += String(analogRead(ANALOG_PIN));
    //s += "<br>"; // Go to the next line.
  //}
  //else
  //{
  //  s += "Invalid Request.<br> Try /led/1, /led/0, or /read.";
  //}
  //s += "</html>\n";

  // Send the response to the client
  client.print(s);
  delay(1);
  Serial.println("Client disconnected");

}

void setupWiFi()
{
  WiFi.mode(WIFI_AP);

  // Do a little work to get a unique-ish name. Append the
  // last two bytes of the MAC (HEX'd) to "Thing-":
  uint8_t mac[WL_MAC_ADDR_LENGTH];
  WiFi.softAPmacAddress(mac);
  String macID = String(mac[WL_MAC_ADDR_LENGTH - 2], HEX) +
                 String(mac[WL_MAC_ADDR_LENGTH - 1], HEX);
  macID.toUpperCase();
  String AP_NameString = "ActivitySensor " + macID;

  char AP_NameChar[AP_NameString.length() + 1];
  memset(AP_NameChar, 0, AP_NameString.length() + 1);

  for (int i=0; i<AP_NameString.length(); i++)
    AP_NameChar[i] = AP_NameString.charAt(i);

  WiFi.softAP(AP_NameChar, WiFiAPPSK);
}

void initHardware()
{
  Serial.begin(115200);
  pinMode(PING_PIN, OUTPUT);
  pinMode(LED_PIN, OUTPUT);
  digitalWrite(LED_PIN, LOW);
  // Don't need to set ANALOG_PIN as input,
  // that's all it can be.
}

