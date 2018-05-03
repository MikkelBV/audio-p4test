#include<Wire.h>
const int MPU = 0x68;  // I2C address of the MPU-9250
int16_t AcX,AcY,AcZ,Tmp,GyX,GyY,GyZ; // Variables for 16bit values

int buttonState = 0;
int buttonPin = 6;
int buttonStateUni;
int state = LOW;
int previous = LOW;
long time = 0;
long debounce = 200;
 
void setup(){
  Wire.begin(); // Initiates the Wire library and joins the I2C in the system (allows communication on the detected USB COM port). Is called as a master by default.
  Wire.beginTransmission(MPU); // Begins the transmission between the master and slave within the I2C system. Takes an address as an argument. All writes to the I2C must be between these calls
  Wire.write(0x6B);  // Accesses the PWR_MGMT_1 register on the sensor (power on /sleep)
  Wire.write(0);     // Sending a 0 to PWR_MGMT_1 wakes up the sensor 
  Wire.endTransmission(true); //Ends transmission, and sends all data to the I2C

  pinMode(buttonPin, INPUT);
  Serial.begin(9600);
}
 
 
void loop(){
  Wire.beginTransmission(MPU); //Begins a new transmission
  Wire.write(0x3B);  // Start accessing the first register (ACCEL_XOUT_H)
  Wire.endTransmission(false); //Ends and restarts the transmission. 
  Wire.requestFrom(MPU,14,true);   // Requests data from the MPU address. Arguments are (Address, quantity of bytes to send, stop=true or false)
                                   // "true" ends the transmission entirely. "false" simply restarts. We want to stop it in this case, since it's loop that begins earlier.
  AcX=Wire.read()<<8|Wire.read();  // 0x3B (ACCEL_XOUT_H) & 0x3C (ACCEL_XOUT_L)    
  AcY=Wire.read()<<8|Wire.read();  // 0x3D (ACCEL_YOUT_H) & 0x3E (ACCEL_YOUT_L)
  AcZ=Wire.read()<<8|Wire.read();  // 0x3F (ACCEL_ZOUT_H) & 0x40 (ACCEL_ZOUT_L)
  Tmp=Wire.read()<<8|Wire.read();  // 0x41 (TEMP_OUT_H) & 0x42 (TEMP_OUT_L)
  GyX=Wire.read()<<8|Wire.read();  // 0x43 (GYRO_XOUT_H) & 0x44 (GYRO_XOUT_L)
  GyY=Wire.read()<<8|Wire.read();  // 0x45 (GYRO_YOUT_H) & 0x46 (GYRO_YOUT_L)
  GyZ=Wire.read()<<8|Wire.read();  // 0x47 (GYRO_ZOUT_H) & 0x48 (GYRO_ZOUT_L)
 
  buttonState = digitalRead(buttonPin);


  if (buttonState == HIGH && previous == LOW && millis() - time > debounce){
    if(state == HIGH){
      state = LOW;
    }
    else{
      state = HIGH;
    }
    time = millis();
  }

  if (state == HIGH){
    buttonStateUni = 1;
  }
  if (state == LOW){
    buttonStateUni = 0;
  }
 
  Serial.print(AcX); Serial.print(";"); Serial.print(AcY); Serial.print(";"); Serial.print(AcZ); Serial.print(";");
  Serial.print(GyX); Serial.print(";"); Serial.print(GyY); Serial.print(";"); Serial.print(GyZ); Serial.print(";");
  Serial.print(buttonStateUni); Serial.println("");
  Serial.flush();

 
  delay(16);
}
 

