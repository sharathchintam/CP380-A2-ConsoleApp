using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using RatingAdjustment.Services;
using System.Collections.Generic;
using BreadmakerReport.Models;

namespace BreadmakerReport
{
    class Program
    {
        static string dbfile = @".\data\breadmakers.db";
        static RatingAdjustmentService ratingAdjustmentService = new RatingAdjustmentService();

        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Bread World");
            var BreadmakerDb = new BreadMakerSqliteContext(dbfile);
            var BMList = BreadmakerDb.Breadmakers
                 .Include(i => i.Reviews)
                .AsEnumerable()
                .Select(s => new {
                    Reviews = s.Reviews.Count,
                    Average = Math.Round(s.Reviews.Average(s => s.stars), 2),
                    Adjust = Math.Round(ratingAdjustmentService.Adjust(s.Reviews.Average(s => s.stars), s.Reviews.Count()), 2),
                    s.title
                })
                .OrderByDescending(od => od.Adjust)
                .ToList();

            Console.WriteLine("[#]  Reviews Average  Adjust    Description");
            for (var j = 0; j < 3; j++)
            {
                var i = BMList[j];
                // TODO: add output
                Console.WriteLine("[{0}] {1} {2} {3} {4}", j + 1, i.Reviews, i.Average, i.Adjust, i.title);
            }
        }
    }
}
