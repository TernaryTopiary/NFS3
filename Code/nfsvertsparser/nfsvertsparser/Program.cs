using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nfsvertsparser
{
    class Program
    {
        class Point3D
        {
            public decimal X, Y, Z;

            public Point3D(decimal x, decimal y, decimal z)
            {
                X = x;
                Y = y;
                Z = z;
            }

            public override string ToString()
            {
                return "v " + X + " " + Y + " " + Z;
            }
        }

        class Quad3D
        {
            public int V1, V2, V3, V4;

            public Quad3D(int v1, int v2, int v3, int v4)
            {
                V1 = v1;
                V2 = v2;
                V3 = v3;
                V4 = v4;
            }

            public override string ToString()
            {
                return "f " + V1 + " " + V2 + " " + V3 + " " + V4;
            }
        }

        /// <summary>
        /// Takes verticies from T3ED and spits out an OBJ.
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            var path = SET THIS;
            var lines = File.ReadAllLines(path).Skip(1).ToList();
            var numBlocks = lines[0];
            lines = lines.Skip(1).ToList();

            var outVerts = new List<string>();
            var outFaces = new List<string>();
            var outputPath = SET THIS;
            
            var index = 0;

            while (lines.Any())
            {
                var vertCount = int.Parse(lines[0]);
                Console.WriteLine("verts: "+ vertCount);
                lines = lines.Skip(1).ToList();
                var chunkVerts = lines.Take(vertCount*4).Where(line => line.StartsWith("X") || line.StartsWith("Y") || line.StartsWith("Z")).ToList();

                var verts = new Point3D[chunkVerts.Count / 3];
                for (var i = 0; i < chunkVerts.Count; i += 3)
                {
                    var x = decimal.Parse(chunkVerts[i].Substring(3));
                    var y = decimal.Parse(chunkVerts[i + 1].Substring(3));
                    var z = decimal.Parse(chunkVerts[i + 2].Substring(3));

                    verts[i / 3] = new Point3D(x, y, z);
                }

                for (var i = 0; i < outVerts.Count; i += 4)
                {
                    outFaces.Add(new Quad3D(i + 1, i + 2, i + 3, i + 4).ToString());
                }

                //sb.Add("o chunk" + index++);
                outVerts.AddRange(verts.Select(v => v.ToString()));
                //outFaces.AddRange(faces.Select(f => f.ToString()));

                lines = lines.Skip(vertCount * 4).ToList();
            }

            for (var i = 0; i < outVerts.Count; i+=4)
            {
                outFaces.Add(new Quad3D(i + 1, i + 2, i + 3, i + 4).ToString());
            }

            Console.Write("done");
            File.WriteAllLines(outputPath, outVerts.Concat(outFaces));
        }
    }
}
