
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Triangles.Models;

namespace Triangles.Controllers
{
    public class TriangleController : Controller
    {
        [HttpGet]
        public IActionResult Info(double side1, double side2, double side3)
        {
            Triangle triangle = new Triangle(side1, side2, side3);
            if (!triangle.IsValid())
            {
                return View("Error");
            }

            double[] sides = { side1, side2, side3 };
            Array.Sort(sides);

            double perimeter = triangle.Perimeter();
            double area = triangle.Area();
            double[] normalizedSides = triangle.NormalizedSides();

            ViewBag.Sides = sides;
            ViewBag.NormalizedSides = normalizedSides;
            ViewBag.Area = area;
            ViewBag.Perimeter = perimeter;

            return View();
        }

        [HttpGet]
        public IActionResult Area(double side1, double side2, double side3)
        {
            Triangle triangle = new Triangle(side1, side2, side3);
            if (!triangle.IsValid())
            {
                return View("Error");
            }

            double area = triangle.Area();
            return Content(area.ToString("F2"));
        }

        [HttpGet]
        public IActionResult Perimeter(double side1, double side2, double side3)
        {
            Triangle triangle = new Triangle(side1, side2, side3);
            if (!triangle.IsValid())
            {
                return View("Error");
            }

            double perimeter = triangle.Perimeter();
            return Content(perimeter.ToString("F0"));
        }

        [HttpGet]
        public IActionResult IsRightAngled(double side1, double side2, double side3)
        {
            Triangle triangle = new Triangle(side1, side2, side3);
            if (!triangle.IsValid())
            {
                return View("Error");
            }

            bool isRightAngled = triangle.IsRightAngled();
            return Content(isRightAngled.ToString().ToLower());
        }

        [HttpGet]
        public IActionResult IsEquilateral(double side1, double side2, double side3)
        {
            Triangle triangle = new Triangle(side1, side2, side3);
            if (!triangle.IsValid())
            {
                return View("Error");
            }

            bool isEquilateral = triangle.IsEquilateral();
            return Content(isEquilateral.ToString().ToLower());
        }

        [HttpGet]
        public IActionResult IsIsosceles(double side1, double side2, double side3)
        {
            Triangle triangle = new Triangle(side1, side2, side3);
            if (!triangle.IsValid())
            {
                return View("Error");
            }

            bool isIsosceles = triangle.IsIsosceles();
            return Content(isIsosceles.ToString().ToLower());
        }

        [HttpGet]
        public IActionResult AreCongruent(
            [FromQuery(Name = "tr1.side1")] double tr1_side1,
            [FromQuery(Name = "tr1.side2")] double tr1_side2,
            [FromQuery(Name = "tr1.side3")] double tr1_side3,
            [FromQuery(Name = "tr2.side1")] double tr2_side1,
            [FromQuery(Name = "tr2.side2")] double tr2_side2,
            [FromQuery(Name = "tr2.side3")] double tr2_side3)
        {
            Triangle triangle1 = new Triangle(tr1_side1, tr1_side2, tr1_side3);
            Triangle triangle2 = new Triangle(tr2_side1, tr2_side2, tr2_side3);
            if (!triangle1.IsValid() || !triangle2.IsValid())
            {
                return View("Error");
            }

            bool areCongruent = triangle1.AreCongruent(triangle2);
            return Content(areCongruent.ToString().ToLower());
        }

        [HttpGet]
        public IActionResult AreSimilar(
            [FromQuery(Name = "tr1.side1")] double tr1_side1,
            [FromQuery(Name = "tr1.side2")] double tr1_side2,
            [FromQuery(Name = "tr1.side3")] double tr1_side3,
            [FromQuery(Name = "tr2.side1")] double tr2_side1,
            [FromQuery(Name = "tr2.side2")] double tr2_side2,
            [FromQuery(Name = "tr2.side3")] double tr2_side3)
        {
            Triangle triangle1 = new Triangle(tr1_side1, tr1_side2, tr1_side3);
            Triangle triangle2 = new Triangle(tr2_side1, tr2_side2, tr2_side3);
            if (!triangle1.IsValid() || !triangle2.IsValid())
            {
                return View("Error");
            }

            bool areSimilar = triangle1.AreSimilar(triangle2);
            return Content(areSimilar.ToString().ToLower());
        }

        [HttpGet]
        public IActionResult InfoGreatestPerimeter([FromQuery] List<TriangleDto> tr)
        {
            List<Triangle> triangles = new List<Triangle>();

            foreach (var triangleDto in tr)
            {
                Triangle triangle = new Triangle(triangleDto.Side1, triangleDto.Side2, triangleDto.Side3);
                if (triangle.IsValid())
                {
                    triangles.Add(triangle);
                }
            }

            if (triangles.Count == 0)
            {
                return View("Error");
            }

            Triangle greatestPerimeterTriangle = triangles.OrderByDescending(t => t.Perimeter()).First();

            return RedirectToAction("Info", new { side1 = greatestPerimeterTriangle.Side1, side2 = greatestPerimeterTriangle.Side2, side3 = greatestPerimeterTriangle.Side3 });
        }

        [HttpGet]
        public IActionResult InfoGreatestArea([FromQuery] List<TriangleDto> tr)
        {
            
            List<Triangle> triangles = new List<Triangle>();

            foreach (var triangleDto in tr)
            {
                Triangle triangle = new Triangle(triangleDto.Side1, triangleDto.Side2, triangleDto.Side3);
                if (triangle.IsValid())
                {
                    triangles.Add(triangle);
                }
            }

            if (triangles.Count == 0)
            {
                return View("Error");
            }

            Triangle greatestAreaTriangle = triangles.OrderByDescending(t => t.Area()).First();

            return RedirectToAction("Info", new { side1 = greatestAreaTriangle.Side1, side2 = greatestAreaTriangle.Side2, side3 = greatestAreaTriangle.Side3 });
        }

        [HttpGet]
        public IActionResult NumbersPairwiseNotSimilar([FromQuery] List<TriangleDto> tr)
        {
            if (tr == null || !tr.Any())
            {
                ModelState.AddModelError(string.Empty, "No triangles provided.");
                return View(new List<dynamic>());
            }

            List<Triangle> triangles = new List<Triangle>();

            foreach (var triangleDto in tr)
            {
                Triangle triangle = new Triangle(triangleDto.Side1, triangleDto.Side2, triangleDto.Side3);
                if (triangle.IsValid())
                {
                    triangles.Add(triangle);
                }
            }

            var nonSimilarPairs = triangles
                .SelectMany((t1, i) => triangles.Skip(i + 1), (t1, t2) => new { t1, t2 })
                .Where(pair => !pair.t1.AreSimilar(pair.t2))
                .ToList<dynamic>();

            return View("NumbersPairwiseNotSimilar", nonSimilarPairs);
        }
    }
    public class TriangleDto
    {
        public double Side1 { get; set; }
        public double Side2 { get; set; }
        public double Side3 { get; set; }
    }
}
