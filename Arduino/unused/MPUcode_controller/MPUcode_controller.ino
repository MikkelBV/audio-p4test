#include<Wire.h>
const int MPU = 0x68;  // I2C address of the MPU-6050
int16_t AcX_c,AcY_c,AcZ_c,Tmp_c,GyX_c,GyY_c,GyZ_c;
 
// value returned is in interval [-32768, 32767] so for normalize multiply GyX and others for gyro_normalizer_factor
// float gyro_normalizer_factor = 1.0f / 32768.0f;
 
void setup(){
  Wire.begin();
  Wire.beginTransmission(MPU);
  Wire.write(0x6B);  // PWR_MGMT_1 register
  Wire.write(0);     // set to zero (wakes up the MPU-6050 (9250 in our case)
  Wire.endTransmission(true);
  Serial.begin(9600);
}
 
 
void loop(){
  Wire.beginTransmission(MPU);
  Wire.write(0x3B);  // starting with register 0x3B (ACCEL_XOUT_H)
  Wire.endTransmission(false);
  Wire.requestFrom(MPU,14,true);  // request a total of 14 registers
  AcX_c=Wire.read()<<8|Wire.read();  // 0x3B (ACCEL_XOUT_H) & 0x3C (ACCEL_XOUT_L)    
  AcY_c=Wire.read()<<8|Wire.read();  // 0x3D (ACCEL_YOUT_H) & 0x3E (ACCEL_YOUT_L)
  AcZ_c=Wire.read()<<8|Wire.read();  // 0x3F (ACCEL_ZOUT_H) & 0x40 (ACCEL_ZOUT_L)
  Tmp_c=Wire.read()<<8|Wire.read();  // 0x41 (TEMP_OUT_H) & 0x42 (TEMP_OUT_L)
  GyX_c=Wire.read()<<8|Wire.read();  // 0x43 (GYRO_XOUT_H) & 0x44 (GYRO_XOUT_L)
  GyY_c=Wire.read()<<8|Wire.read();  // 0x45 (GYRO_YOUT_H) & 0x46 (GYRO_YOUT_L)
  GyZ_c=Wire.read()<<8|Wire.read();  // 0x47 (GYRO_ZOUT_H) & 0x48 (GYRO_ZOUT_L)
 

 
  Serial.print(AcX_c); Serial.print(";"); Serial.print(AcY_c); Serial.print(";"); Serial.print(AcZ_c); Serial.print(";");
  Serial.print(GyX_c); Serial.print(";"); Serial.print(GyY_c); Serial.print(";"); Serial.print(GyZ_c); Serial.println("");
  Serial.flush();
 
 //if we want the accelerometer or temperature graphed as well, just do the same as below for those variables, or change the variables.

 /*
 Serial.print(GyX_c, DEC);
 Serial.print(" ");
 Serial.print(GyY_c, DEC);
 Serial.print(" ");
 Serial.println(GyZ_c, DEC);
 */
  delay(16);
}
 

