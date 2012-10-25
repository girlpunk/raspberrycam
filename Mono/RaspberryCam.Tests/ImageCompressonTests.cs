﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using NUnit.Framework;
using RaspberryCam.Tests.Compression;

namespace RaspberryCam.Tests
{
    [TestFixture]
    public class ImageCompressonTests
    {
        const int BlockSize = 16;

        [Test]
        public void When_drawing_image()
        {
            var bytes1 = File.ReadAllBytes("Files/webcam_screenshot1.jpg");

            var stopwatch = Stopwatch.StartNew();

            var memoryStream = new MemoryStream(bytes1);
            var image = Image.FromStream(memoryStream);
            var bitmap = new Bitmap(image);

            var blocksBuilder = new BlocksBuilder(bitmap);
            //var pixelsBlocks = blocksBuilder.GetBlocks();
            //var pixelsBlocks = blocksBuilder.GetBlocksFast();

            var pixelsBlocks = new List<PixelsBlock>();

            bool altern = false;
            for (int i = 0; i < blocksBuilder.GetBlockCount(); i++)
            {
                altern = !altern;
                if (altern)
                    continue;
                var block = blocksBuilder.GetPixelsBlock(i);
                pixelsBlocks.Add(block);
            }

            var output = new Bitmap(bitmap.Width, bitmap.Height);

            foreach (var block in pixelsBlocks)
            {
                foreach (var pixel in block.Pixels)
                {
                    output.SetPixel(pixel.X, pixel.Y, pixel.Color);
                }
            }
            
            output.Save("out1.bmp");

            stopwatch.Stop();
            var elapsed = stopwatch.Elapsed;   
        }

        [Test]
        public void When_generating_combinaisons()
        {
            var stopwatch = Stopwatch.StartNew();

            var combinaisons = new List<ColorCombinaison>();

            Int64 count = 0;

            for (int x = 0; x < BlockSize; x++)
            {
                for (int y = 0; y < BlockSize; y++)
                {
                    for (byte red = 0; red < 255; red++)
                    {
                        for (byte green = 0; green < 255; green++)
                        {
                            for (byte blue = 0; blue < 255; blue++)
                            {
                                //combinaisons.Add(new ColorCombinaison
                                //                        {
                                //                            Red = red,
                                //                            Green = green,
                                //                            Blue = blue
                                //                        });

                                count++;
                            }
                        }
                    }
                }
            }

            stopwatch.Stop();
            var elapsed = stopwatch.Elapsed;   
        }

        [Test]
        public void When_building_blocks()
        {
            var bytes1 = File.ReadAllBytes("Files/webcam_screenshot1.jpg");

            var stopwatch = Stopwatch.StartNew();

            var memoryStream = new MemoryStream(bytes1);
            var image = Image.FromStream(memoryStream);
            var bitmap = new Bitmap(image);

            //var graphics = Graphics.FromImage(bitmap);


            var blocksBuilder = new BlocksBuilder(bitmap);
            var pixelsBlocks = blocksBuilder.GetBlocks();

            //var imageData = blocksBuilder.GetImageData();

            var pixelsBlocks2 = blocksBuilder.GetBlocksFast();


            stopwatch.Stop();

            var elapsed = stopwatch.Elapsed;

            var fps = 1000/stopwatch.ElapsedMilliseconds;

            Assert.AreEqual(pixelsBlocks.First().Pixels.First().Color, pixelsBlocks2.First().Pixels.First().Color);

            Assert.IsTrue(pixelsBlocks.All(b => b.Pixels.Count == BlocksBuilder.BlockSize*BlocksBuilder.BlockSize));

            //for (int i = 0; i < pixelsBlocks.Count; i++)
            //{
            //    for (int j = 0; j < pixelsBlocks[i].Pixels.Count; j++)
            //    {
            //        Assert.AreEqual(pixelsBlocks[i].Pixels[j].X, pixelsBlocks2[i].Pixels[j].X);
            //        Assert.AreEqual(pixelsBlocks[i].Pixels[j].Y, pixelsBlocks2[i].Pixels[j].Y);
            //        Assert.AreEqual(pixelsBlocks[i].Pixels[j].Color, pixelsBlocks2[i].Pixels[j].Color);
            //    }
            //}

        }
    }
}
