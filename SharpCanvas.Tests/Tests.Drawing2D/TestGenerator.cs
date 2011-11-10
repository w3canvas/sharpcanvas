using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using System.Reflection.Emit;
using SharpCanvas.Forms;

namespace SharpCanvas.Tests.Forms
{
    public delegate void TestHandler(Graphics g, Pen stroke, Fill fill);

    public class TestGenerator
    {
        #region Delegates

        public delegate void ArgumentGeneratorHandler(ILGenerator gen);

        #endregion

        private readonly Random angleRandom = new Random();
        private readonly Random boolRandom = new Random();
        private readonly Random colorRandom = new Random();
        private readonly Random coordinateRandom = new Random();

        private int _height;
        private int _width;

        private Dictionary<string, ArgumentGeneratorHandler[]> testMethods;
        private Random testRandom;

        /// <summary>
        /// Generate test method.
        /// We need to know width and height in order to initialize coordinate randomizer (limit it with max value);
        /// </summary>
        public TestHandler GenerateTest(int width, int height)
        {
            _width = width;
            _height = height;
            //collection with methods name to test (we can't go through all methods and properties inside CanvasRenderingContext2D
            //because not all of them are ready). Also we have to specify argument generator for each argument. 
            testMethods = new Dictionary<string, ArgumentGeneratorHandler[]>
                              {
                                  {
                                      "lineTo",
                                      new ArgumentGeneratorHandler[]
                                          {
                                              GenerateCoordinate, GenerateCoordinate
                                          }
                                      },
                                  {
                                      "moveTo",
                                      new ArgumentGeneratorHandler[]
                                          {
                                              GenerateCoordinate, GenerateCoordinate
                                          }
                                      },
                                  {
                                      "strokeRect",
                                      new ArgumentGeneratorHandler[]
                                          {
                                              GenerateCoordinate, GenerateCoordinate, GenerateCoordinate,
                                              GenerateCoordinate
                                          }
                                      },
                                  {
                                      "fillRect",
                                      new ArgumentGeneratorHandler[]
                                          {
                                              GenerateCoordinate, GenerateCoordinate, GenerateCoordinate,
                                              GenerateCoordinate
                                          }
                                      },
                                  {
                                      "clearRect",
                                      new ArgumentGeneratorHandler[]
                                          {
                                              GenerateCoordinate, GenerateCoordinate, GenerateCoordinate,
                                              GenerateCoordinate
                                          }
                                      },
                                  {
                                      "quadraticCurveTo",
                                      new ArgumentGeneratorHandler[]
                                          {
                                              GenerateCoordinate, GenerateCoordinate, GenerateCoordinate,
                                              GenerateCoordinate
                                          }
                                      },
                                  {
                                      "bezierCurveTo",
                                      new ArgumentGeneratorHandler[]
                                          {
                                              GenerateCoordinate, GenerateCoordinate, GenerateCoordinate,
                                              GenerateCoordinate, GenerateCoordinate, GenerateCoordinate
                                          }
                                      },
                                  {
                                      "arcTo",
                                      new ArgumentGeneratorHandler[]
                                          {
                                              GenerateCoordinate, GenerateCoordinate, GenerateCoordinate,
                                              GenerateCoordinate, GenerateCoordinate
                                          }
                                      },
                                  {
                                      "rect",
                                      new ArgumentGeneratorHandler[]
                                          {
                                              GenerateCoordinate, GenerateCoordinate, GenerateCoordinate,
                                              GenerateCoordinate
                                          }
                                      },
                                  {
                                      "arc",
                                      new ArgumentGeneratorHandler[]
                                          {
                                              GenerateCoordinate, GenerateCoordinate, GenerateCoordinate, GenerateAngle,
                                              GenerateAngle, GenerateBool
                                          }
                                      },
                                  {
                                      "set_StrokeStyle",
                                      new ArgumentGeneratorHandler[]
                                          {
                                              GenerateColor
                                          }
                                      },
                                  {
                                      "set_FillStyle",
                                      new ArgumentGeneratorHandler[]
                                          {
                                              GenerateColor
                                          }
                                      },
                                  {"beginPath", new ArgumentGeneratorHandler[] {}},
                                  {"closePath", new ArgumentGeneratorHandler[] {}},
                                  {"stroke", new ArgumentGeneratorHandler[] {}},
                                  {"fill", new ArgumentGeneratorHandler[] {}}
                              };

            var method = new DynamicMethod(string.Empty, typeof (void),
                                           new[] {typeof (Graphics), typeof (Pen), typeof (Fill)}, typeof (FormHost));

            ConstructorInfo canvasCtor = typeof (CanvasRenderingContext2D).GetConstructor(
                new[] {typeof (Graphics), typeof (Pen), typeof (Fill)});

            ILGenerator gen = method.GetILGenerator();

            gen.Emit(OpCodes.Nop);

            //create new CanvasRenderingContext2D
            gen.DeclareLocal(typeof (CanvasRenderingContext2D));
            gen.Emit(OpCodes.Ldarg_0);
            gen.Emit(OpCodes.Ldarg_1);
            gen.Emit(OpCodes.Ldarg_2);
            gen.Emit(OpCodes.Newobj, canvasCtor);
            gen.Emit(OpCodes.Stloc_0);

            //init test randomizer
            testRandom = new Random(testMethods.Count - 1);

            //generate test methods calls
            var amountOfMethodsToCallRandom = new Random();
            int amountOfMethodsToCall = amountOfMethodsToCallRandom.Next(300);
            while (amountOfMethodsToCall-- > 0)
            {
                int methodIndex = testRandom.Next(0, testMethods.Count);
                string methodName = string.Empty;
                foreach (KeyValuePair<string, ArgumentGeneratorHandler[]> pair in testMethods)
                {
                    if(methodIndex-- == 0)
                    {
                        methodName = pair.Value.ToString();
                    }
                }
                
                //get CanvasRenderingContext2D instance
                gen.Emit(OpCodes.Ldloc_0);
                foreach (ArgumentGeneratorHandler generateArgument in testMethods[methodName])
                {
                    //generate argument and put it to the stack
                    generateArgument(gen);
                }
                gen.Emit(OpCodes.Call, GetMethodInfo(methodName));
            }

            //Call stroke method. We need to call it after all drawings in order to show them on the form
            gen.Emit(OpCodes.Ldloc_0);
            gen.Emit(OpCodes.Call, (typeof (CanvasRenderingContext2D)).GetMethod("stroke"));
            gen.Emit(OpCodes.Nop);
            gen.Emit(OpCodes.Ret);

            var handler = method.CreateDelegate(typeof (TestHandler)) as TestHandler;
            return handler;
        }

        /// <summary>
        /// We should take in count not only method name, but number of arguments too, because we have
        /// overloaded methods such as strokeRect. We do not take in count types of arguments for now.
        /// </summary>
        private MethodInfo GetMethodInfo(string methodName)
        {
            MethodInfo[] methods = typeof (CanvasRenderingContext2D).GetMethods();
            foreach (MethodInfo methodInfo in methods)
            {
                if (methodInfo.Name == methodName && methodInfo.GetParameters().Length == testMethods[methodName].Length)
                {
                    return methodInfo;
                }
            }
            return null;
        }

        /// <summary>
        /// Each coordinate should be in visible area. Current method implementation doesn't achive the goal, but it allows draw images mostly 
        /// inside visible area and do not separate this generator into two: for X and for Y...
        /// </summary>
        public void GenerateCoordinate(ILGenerator gen)
        {
            gen.Emit(OpCodes.Ldc_R4, (float) coordinateRandom.Next(0, Math.Max(_width, _height)));
        }

        /// <summary>
        /// Generates color. 
        /// TODO: implement generation of rgba
        /// </summary>
        public void GenerateColor(ILGenerator gen)
        {
            gen.Emit(OpCodes.Ldstr,
                     "rgb(" + colorRandom.Next(0, 255) + "," + colorRandom.Next(0, 255) + "," + colorRandom.Next(0, 255) +
                     ")");
        }

        /// <summary>
        /// Generates arbitrary angle
        /// </summary>
        public void GenerateAngle(ILGenerator gen)
        {
            gen.Emit(OpCodes.Ldc_R4, (float) Math.PI/angleRandom.Next(0, 360));
        }

        /// <summary>
        /// Generates boolean value
        /// </summary>
        public void GenerateBool(ILGenerator gen)
        {
            gen.Emit(OpCodes.Ldc_R4, boolRandom.Next(0, 1));
        }
    }
}