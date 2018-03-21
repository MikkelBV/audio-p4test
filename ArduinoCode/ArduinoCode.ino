const int buttonPin1 = 6;
const int buttonPin2 = 7;

void setup(){
  Serial.begin(9600);

  pinMode(buttonPin1, INPUT);
  pinMode(buttonPin2, INPUT);

  digitalWrite(buttonPin1, HIGH);
  digitalWrite(buttonPin2, HIGH);
}

void loop(){
  if(digitalRead(buttonPin1) == LOW){
    Serial.write(1);
    Serial.flush();
    delay(20);
  }
  if(digitalRead(buttonPin2) == LOW){
    Serial.write(2);
    Serial.flush();
    delay(20);
  }
}

