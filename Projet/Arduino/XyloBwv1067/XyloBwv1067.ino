#include <Wire.h>

struct Mcp23017Register
{
  byte address, registerAddress;
};

const Mcp23017Register Mcp23017Registers[] = 
{
  { 33, 0x12 }, 
  { 34, 0x12 }, 
  { 32, 0x12 }, 
  { 33, 0x13 }, 
  { 32, 0x13 }
};

const int RegisterCount = sizeof(Mcp23017Registers) / sizeof(Mcp23017Register);

byte Mcp23017RegisterValues[] { 0, 0, 0, 0, 0 };

struct Tone
{
  byte registerIndex, mask;
};

struct Note
{
  long tick;
  const Tone& tone;
};

const Tone Do5 = { 0, 128 };
const Tone Re5 = { 0, 64 };
const Tone Mi5 = { 0, 32 };
const Tone Fa5 = { 0, 16 };
const Tone Sol5 = { 0, 8 };
const Tone La6 = { 0, 4 };
const Tone Si6 = { 0, 2 };
const Tone Do6 = { 0, 1 };

const Tone Re6 = { 1, 128 };
const Tone Mi6 = { 1, 64 };
const Tone Fa6 = { 1, 32 };
const Tone Sol6 = { 1, 16 };
const Tone La7 = { 1, 8 };
const Tone Si7 = { 1, 4 };

const Tone Do7 = { 2, 128 };
const Tone Re7 = { 2, 64 };
const Tone Mi7 = { 2, 32 };
const Tone Fa7 = { 2, 16 };
const Tone Sol7 = { 2, 8 };
const Tone La8 = { 2, 4 };
const Tone Si8 = { 2, 2 };
const Tone Do8 = { 2, 1 };

const Tone DoDiese5 = { 3, 1 };
const Tone ReDiese5 = { 3, 2 };
const Tone FaDiese5 = { 3, 4 };
const Tone SolDiese5 = { 3, 8 };
const Tone LaDiese6 = { 3, 16 };
const Tone DoDiese6 = { 3, 32 };
const Tone ReDiese6 = { 3, 64 };
const Tone FaDiese6 = { 3, 128 };

const Tone SolDiese6 = { 4, 1 };
const Tone LaDiese7 = { 4, 2 };
const Tone DoDiese7 = { 4, 4 };
const Tone ReDiese7 = { 4, 8 };
const Tone FaDiese7 = { 4, 16 };
const Tone SolDiese7 = { 4, 32 };
const Tone LaDiese8 = { 4, 64 };

const Tone& MiDiese5 = Fa5;
const Tone& SiDiese6 = Do6;
const Tone& MiDiese6 = Fa6;
const Tone& SiDiese7 = Do7;
const Tone& MiDiese7 = Fa7;
const Tone& SiDiese8 = Do8;
 
const Tone& ReBemol5 = DoDiese5;
const Tone& MiBemol5 = ReDiese5;
const Tone& FaBemol5 = Mi5;
const Tone& SolBemol5 = FaDiese5;
const Tone& LaBemol6 = SolDiese5;
const Tone& SiBemol6 = LaDiese6;
const Tone& DoBemol6 = Si6;

const Tone& ReBemol6 = DoDiese6;
const Tone& MiBemol6 = ReDiese6;
const Tone& FaBemol6 = Mi6;
const Tone& SolBemol6 = FaDiese6;
const Tone& LaBemol7 = SolDiese6;
const Tone& SiBemol7 = LaDiese7;
const Tone& DoBemol7 = Si7;

const Tone& ReBemol7 = DoDiese7;
const Tone& MiBemol7 = ReDiese7;
const Tone& FaBemol7 = Mi7;
const Tone& SolBemol7 = FaDiese7;
const Tone& LaBemol8 = SolDiese7;
const Tone& SiBemol8 = LaDiese8;
const Tone& DoBemol8 = Si8;

void mcp23017SetIoDirOutput(byte address)
{
  Wire.beginTransmission(address);
  Wire.write(0);
  Wire.write(0);
  Wire.endTransmission();
  Wire.beginTransmission(address);
  Wire.write(1);
  Wire.write(0);
  Wire.endTransmission();
}

void setup() {
  Wire.begin(); // join i2c bus (address optional for master)
  //Wire.setClock(400000L); 
  mcp23017SetIoDirOutput(32);
  mcp23017SetIoDirOutput(33);
  mcp23017SetIoDirOutput(34);
}

void preparePush(const Tone& tone)
{
  Mcp23017RegisterValues[tone.registerIndex] |= tone.mask;
}

void applyPush()
{
  for (int i = 0; i < RegisterCount; i++)
  {
//    if (Mcp23017RegisterValues[i] != 0)
    {
      Wire.beginTransmission(Mcp23017Registers[i].address);
      Wire.write(Mcp23017Registers[i].registerAddress);
      Wire.write(Mcp23017RegisterValues[i]  | (i==4 ? 0x80 : 0));
      Wire.endTransmission();    
    }
  }
}

void releasePush()
{
  for (int i = 0; i < RegisterCount; i++)
  {
//    if (Mcp23017RegisterValues[i] != 0)
    {
      Wire.beginTransmission(Mcp23017Registers[i].address);
      Wire.write(Mcp23017Registers[i].registerAddress);
//      Wire.write(0);
      Wire.write(0 | (i ==4 ? 0x80: 0));
      Wire.endTransmission(); 
      Mcp23017RegisterValues[i] = 0;
    }
  }
}

const Note Melody[] =
{
  // Mesure 1
  { 8, La8},
  { 8, Do5},
  { 8, Mi5},
  { 8, La6},

  { 12, Do8},
  { 14, La8},

  // Mesure 2
  { 16, Mi7 },
  { 16, Do5 },
  { 16, Mi5 },
  { 16, La6 },

  { 20, La8 },
  { 22, Mi7 },
  
  { 24, Do7 },
  { 24, Mi5 },
  
  { 28, Mi7 },
  { 28, Do5 },

  { 30, Do7 },

  // Mesure 3

  { 32, La7 },
  { 32, La6 },
  
  { 36, Mi5 },
  
  { 40, Mi6 },
  { 40, Do5 },

  { 42, La7 },
  
  { 44, Do7 },
  // { 44, La5 },

  { 46, La7 },

  // Mesure 4

  { 48, Si7 },
  { 48, Re5 },

  { 50, La7 },
  
  { 52, Si7 },
  // { 52, Si5 },
  
  { 54, La7 },

  { 56, SolDiese6 },
  { 56, Mi5 },
  
  { 58, Si7 },

  { 60, Re7 },
  // { 60, Mi4 }

  { 62, Si7 },

  // Mesure 5

  { 64, Do7 },
  // { 64, La5 }

  // { 66, Si5}

  { 68, La7 },
  { 68, Do5 },

  // { 70, Si5 }
  
  { 72, La8},
  { 72, Do5},
  { 72, Mi5},
  { 72, La6},

  { 76, Do8},
  { 78, La8},

  // Mesure 6
  { 80, Mi7 },
  { 80, Do5 },
  { 80, Mi5 },
  { 80, La6 },

  { 84, La8 },
  { 86, Mi7 },
  
  { 88, Do7 },
  { 88, Mi5 },
  
  { 92, Mi7 },
  { 92, Do5 },

  { 94, Do7 },

  // Mesure 7

  { 96, La7 },
  { 96, La6 },

  { 100, Mi6},

  { 104, Do7 },
  { 104, Do5 },

  { 108, Do7 },
  // { 108, La5}, 

  // Mesure 8

  { 112, Do7 },
  { 112, FaDiese5 },

  { 116, Do7 },
  { 116, La6 },

  { 118, FaDiese5 },
  
  { 120, La7 },
  { 120, Re5 },

  { 124, Do7 }, 
  { 124, FaDiese5 },

  // Mesure 9

  { 128, Do7 },
  { 128, Sol5 },

  { 129, Re7 },
  
  { 130, Do7 },

  { 132, Si7 },
  // { 132, Sol4 }

  { 136, Mi7 },
  { 136, Mi5 },
  
  { 140, Mi7 },
  { 140, Do5 },

  // Mesure 10

  { 144, Mi7 },
  { 144, La6 },

  { 148, Mi7 },
  { 148, Do6 },

  { 150, La6 },

  { 152, Do8 },
  { 152, FaDiese5 },

  { 156, Mi7 },
  { 156, La6 },

  // Mesure 11

  { 160, Mi7 }, 
  { 160, Si6 },

  { 161, FaDiese7 },

  { 162, Mi7 },

  { 164, ReDiese7 },
  // { 164, Si5 }

  { 168, Si7 },
  { 168, Sol5 },

  { 170, Mi7 },
  
  { 172, Si7 },
  { 172, Mi5 },

  { 174, Mi7 },

  // Mesure 12

  { 176, FaDiese7 },
  { 176, La6 },

  { 178, Mi7 },

  { 180, FaDiese7 },
  { 180, FaDiese5 },

  { 182, Mi7 },

  { 184, ReDiese7 },
  { 184, Si6 },

  { 186, FaDiese7 },

  { 188, La7 },
  // { 188, Si5 }

  { 190, FaDiese7 },
  
  // Mesure 13

  { 192, Sol7 },
  { 192, Mi5 },

  { 194, FaDiese7 },

  { 196, Sol7 },
  // { 196, Sol5 }

  { 198, FaDiese7 },

  { 200, Mi7 },
  { 200, Mi5 } ,

  { 202, Sol7 },
  
  { 204, Mi7 },
  { 204, Sol5 },

  { 206, ReDiese7 },

  // Mesure 14

  { 208, Mi7 },
  { 208, Do5 },

  { 210, Do8 },

  { 212, Mi7 },
  { 212, La6 },

  { 214, ReDiese7 },

  { 216, Mi7 },
  // { 216, Si5 }

  { 218, Si7 },

  { 220, Mi7 },
  { 220, Si6 },

  { 222, Re7 },

  // Mesure 15

  { 224, Mi7 },
  // { 224, La5 },

  { 226, Do8 },
  
  { 228, Mi7 },
  { 228, Do5 },

  { 230, ReDiese7 },
  
  { 232, Mi7 },
  { 232, La6 },

  { 234, Do8 },

  { 236, Si8 },
  { 236, FaDiese5 },

  { 238, La7 },

  // Mesure 16

  { 240, Si8 },
  { 240, Sol5 },

  { 242, Sol7 },

  { 244, FaDiese7 },
  { 244, La6 },

  { 246, Mi7 },

  { 248, Sol7 },
  { 248, Si6 },

  { 252, FaDiese7 },
  // {252, Si5 },

  { 253, Sol7 },

  { 254, FaDiese7 },

  { 256, Mi7 },
  { 256, Mi5 },
  { 256, Sol5 },
  
  // Reserved end
  { -1, Do5 }
};

const int MelodyNoteCount = sizeof(Melody) / sizeof(Note);

const int PushTimeMicros = 8500;
const int TempoTick = 55;//valeur par défaut 75

long tick = 0;

long noteIndex = 0;

void loop() 
{
  while (Melody[noteIndex].tick == tick)
  {
    preparePush(Melody[noteIndex].tone);
    noteIndex++;
  }
  applyPush();
  delayMicroseconds(PushTimeMicros);
  releasePush();
  delay(TempoTick - PushTimeMicros / 1000);
  tick++;
  if (Melody[noteIndex].tick == -1)
  {
    delay(100);
    noteIndex = 0;
    tick = 0;
  }
  
}
