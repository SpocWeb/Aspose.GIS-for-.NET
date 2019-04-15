﻿using Aspose.Gis;
using Aspose.Gis.Rendering;
using Aspose.Gis.Rendering.Symbolizers;
using Aspose.GIS.Examples.CSharp;
using System;
using System.Drawing;

namespace Aspose.GIS_for.NET.Rendering
{
    public class RenderMap
    {
        private static string dataDir = RunExamples.GetDataDir();

        public static void Run()
        {
            // Note: a license is required to run this example. 
            // You can request a 30-day temporary license here: https://purchase.aspose.com/temporary-license
            var pathToLicenseFile = ""; // <- change this to the path to your license file
            var license = new License();
            license.SetLicense(pathToLicenseFile);

            RenderWithDefaultSettings();
            AddMapLayersAndStyles();

            ChangeMarkerStyle();
            ChangeMarkerStyleTriangles();
            ChangeMarkerStyleFeatureBased();

            ChangeLineStyle();
            ChangeLineStyleComplex();

            ChangeFillStyle();

            MixedGeometryRendering();
            LayeredSymbolizers();
            RuleBasedRendering();

        }

        public static void RenderWithDefaultSettings()
        {
            //ExStart: RenderWithDefaultSettings
            using (var map = new Map(800, 400))
            {
                map.Add(VectorLayer.Open(dataDir + "land.shp", Drivers.Shapefile));
                map.Render(dataDir + "land_out.svg", Renderers.Svg);
            }
            //ExEnd: RenderWithDefaultSettings
        }

        public static void AddMapLayersAndStyles()
        {
            //ExStart: AddMapLayersAndStyles
            using (var map = new Map(800, 476))
            {
                var baseMapSymbolizer = new SimpleFill { FillColor = Color.Salmon, StrokeWidth = 0.75 };
                map.Add(VectorLayer.Open(dataDir + "basemap.shp", Drivers.Shapefile), baseMapSymbolizer);

                var citiesSymbolizer = new SimpleMarker() { FillColor = Color.LightBlue };
                citiesSymbolizer.FeatureBasedConfiguration = (feature, symbolizer) =>
                {
                    var population = feature.GetValue<int>("population");
                    symbolizer.Size = 10 * population / 1000;
                    if (population < 2500)
                    {
                        symbolizer.FillColor = Color.GreenYellow;
                    }
                };
                map.Add(VectorLayer.Open(dataDir + "points.geojson", Drivers.GeoJson), citiesSymbolizer);

                map.Render(dataDir + "cities_out.svg", Renderers.Svg);
            }
            //ExStart: AddMapLayersAndStyles
        }

        #region Marker

        public static void ChangeMarkerStyle()
        {
            //ExStart: ChangeMarkerStyle
            using (var map = new Map(500, 200))
            {
                var symbol = new SimpleMarker() { Size = 7, StrokeWidth = 1, FillColor = Color.Red };

                map.Add(VectorLayer.Open(dataDir + "points.geojson", Drivers.GeoJson), symbol);
                map.Render(dataDir + "points_out.svg", Renderers.Svg);
            }
            //ExEnd: ChangeMarkerStyle
        }

        public static void ChangeMarkerStyleTriangles()
        {
            //ExStart: ChangeMarkerStyleTriangles
            using (var map = new Map(500, 200))
            {
                var symbol = new SimpleMarker()
                {
                    Size = 15,
                    FillColor = Color.DarkMagenta,
                    StrokeStyle = StrokeStyle.None,
                    ShapeType = MarkerShapeType.Triangle,
                    Rotation = 90
                };

                map.Add(VectorLayer.Open(dataDir + "points.geojson", Drivers.GeoJson), symbol);
                map.Render(dataDir + "points_out.svg", Renderers.Svg);
            }
            //ExEnd: ChangeMarkerStyleTriangles
        }

        public static void ChangeMarkerStyleFeatureBased()
        {
            //ExStart: ChangeMarkerStyleFeatureBased
            using (var map = new Map(500, 200))
            {
                var symbol = new SimpleMarker() { FillColor = Color.LightBlue };
                symbol.FeatureBasedConfiguration = (feature, symbolizer) =>
                {
                    // retirieve city population (in thousands) from a feature attribute
                    var population = feature.GetValue<int>("population");

                    // let's increase circle radius by 5 pixels for each million people
                    symbolizer.Size = 5 * population / 1000;

                    // and let's draw cities with less than 2.5M people in green
                    if (population < 2500)
                    {
                        symbolizer.FillColor = Color.GreenYellow;
                    }
                };

                map.Add(VectorLayer.Open(dataDir + "points.geojson", Drivers.GeoJson), symbol);
                map.Render(dataDir + "points_out.svg", Renderers.Svg);
            }
            //ExEnd: ChangeMarkerStyleFeatureBased
        }

        #endregion

        #region Line

        public static void ChangeLineStyle()
        {
            //ExStart: ChangeLineStyle
            using (var map = new Map(500, 317))
            {
                var symbolizer = new SimpleLine { Width = 1.5, Color = Color.FromArgb(0xAE, 0xD9, 0xFD) };

                map.Add(VectorLayer.Open(dataDir + "lines.geojson", Drivers.GeoJson), symbolizer);
                map.Render(dataDir + "lines_out.svg", Renderers.Svg);
            }
            //ExEnd: ChangeLineStyle
        }

        public static void ChangeLineStyleComplex()
        {
            //ExStart: ChangeLineStyleComplex
            using (var map = new Map(500, 317))
            {
                var lineSymbolizer = new SimpleLine { Width = 1.5, Color = Color.FromArgb(0xae, 0xd9, 0xfd) };
                lineSymbolizer.FeatureBasedConfiguration = (feature, symbolizer) =>
                {
                    if (feature.GetValue<string>("NAM") == "UNK")
                    {
                        symbolizer.Width = 1;
                        symbolizer.Style = StrokeStyle.Dash;
                    }
                };

                map.Add(VectorLayer.Open(dataDir + "lines.geojson", Drivers.GeoJson), lineSymbolizer);
                map.Render(dataDir + "lines_out.svg", Renderers.Svg);
            }
            //ExEnd: ChangeLineStyleComplex
        }

        #endregion

        #region Polygons

        public static void ChangeFillStyle()
        {
            //ExStart: ChangeFillStyle
            using (var map = new Map(500, 450))
            {
                var symbolizer = new SimpleFill { FillColor = Color.Azure, StrokeColor = Color.Brown };

                map.Add(VectorLayer.Open(dataDir + "polygons.geojson", Drivers.GeoJson), symbolizer);
                map.Render(dataDir + "polygons_out.svg", Renderers.Svg);
            }
            //ExEnd: ChangeFillStyle
        }

        #endregion

        #region Mixed Geometry Symbolizer

        public static void MixedGeometryRendering()
        {
            //ExStart: MixedGeometryRendering
            using (var map = new Map(500, 210))
            {
                var symbolizer = new MixedGeometrySymbolizer();
                symbolizer.PointSymbolizer = new SimpleMarker { FillColor = Color.Red, Size = 10 };
                symbolizer.LineSymbolizer = new SimpleLine { Color = Color.Blue };
                symbolizer.PolygonSymbolizer = new SimpleFill { FillColor = Color.Green };

                map.Add(VectorLayer.Open(dataDir + "mixed.geojson", Drivers.GeoJson), symbolizer);
                map.Render(dataDir + "mixed_out.svg", Renderers.Svg);
            }
            //ExEnd: MixedGeometryRendering
        }

        #endregion

        #region Layered Symbolizer

        public static void LayeredSymbolizers()
        {
            //ExStart: LayeredSymbolizers
            using (var map = new Map(200, 200))
            {
                var symbolizer = new LayeredSymbolizer(RenderingOrder.ByFeatures);
                symbolizer.Add(new SimpleLine { Width = 10, Color = Color.Black });
                symbolizer.Add(new SimpleLine { Width = 8, Color = Color.White });

                map.Add(VectorLayer.Open(dataDir + "intersection.geojson", Drivers.GeoJson), symbolizer);
                map.Render(dataDir + "intersection_out.svg", Renderers.Svg);
            }
            //ExEnd: LayeredSymbolizers
        }

        #endregion

        #region Rule-based Symbolizer

        public static void RuleBasedRendering()
        {
            //ExStart: RuleBasedRendering
            using (var map = new Map(600, 400))
            {
                var symbolizer = new RuleBasedSymbolizer();
                symbolizer.Add(f => f.GetValue<string>("sov_a3") == "CAN", new SimpleLine { Color = Color.FromArgb(213, 43, 30) });
                symbolizer.Add(f => f.GetValue<string>("sov_a3") == "USA", new SimpleLine { Color = Color.FromArgb(15, 71, 175) });
                symbolizer.Add(f => f.GetValue<string>("sov_a3") == "MEX", new SimpleLine { Color = Color.FromArgb(0, 104, 71) });
                symbolizer.Add(f => f.GetValue<string>("sov_a3") == "CUB", new SimpleLine { Color = Color.FromArgb(255, 215, 0) });

                map.Add(VectorLayer.Open(dataDir + "railroads.shp", Drivers.Shapefile), symbolizer);
                map.Render(dataDir + "railroads_out.svg", Renderers.Svg);
            }
            //ExEnd: RuleBasedRendering
        }

        #endregion
    }
}