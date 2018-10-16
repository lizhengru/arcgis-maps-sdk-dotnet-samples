// Copyright 2018 Esri.
//
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License.
// You may obtain a copy of the License at: http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an 
// "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the License for the specific 
// language governing permissions and limitations under the License.

using Esri.ArcGISRuntime.Data;
using Esri.ArcGISRuntime.Geometry;
using Esri.ArcGISRuntime.Mapping;
using Xamarin.Forms;

namespace ArcGISRuntime.Samples.ShowLabelsOnLayer
{
    [ArcGISRuntime.Samples.Shared.Attributes.Sample(
        "Show labels on layer",
        "Layers",
        "Show labels on a feature layer using a JSON label definition.",
        "The labeling of the names on the US Highways layer is accomplished by supplying a JSON string to the FeatureLayer's LabelDefinition. The JSON is based on the new ArcGIS web map specification.",
        "")]
    public partial class ShowLabelsOnLayer : ContentPage
    {
        public ShowLabelsOnLayer()
        {
            InitializeComponent();

            Title = "Show labels on layer";

            // Create the UI, setup the control references and execute initialization 
            Initialize();
        }

        private async void Initialize()
        {
            // Create a map with a light gray canvas basemap.
            Map sampleMap = new Map(Basemap.CreateLightGrayCanvas());

            // Assign the map to the MapView.
            MyMapView.Map = sampleMap;

            // Define the URL string for the feature layer.
            string layerUrl = "https://services.arcgis.com/P3ePLMYs2RVChkJx/arcgis/rest/services/USA_115th_Congressional_Districts/FeatureServer/0";

            // Create a service feature table from the URL.
            ServiceFeatureTable featureTable = new ServiceFeatureTable(new System.Uri(layerUrl));

            // Create a feature layer from the service feature table.
            FeatureLayer districtFeatureLabel = new FeatureLayer(featureTable);

            // Add the feature layer to the operations layers collection of the map.
            sampleMap.OperationalLayers.Add(districtFeatureLabel);

            // Load the feature layer - this way we can obtain it's extent.
            await districtFeatureLabel.LoadAsync();

            // Zoom the map view to the extent of the feature layer.
            await MyMapView.SetViewpointCenterAsync(new MapPoint(-10846309.950860, 4683272.219411, SpatialReferences.WebMercator), 20000000);

            // Help regarding the Json syntax for defining the LabelDefinition.FromJson syntax can be found here:
            // https://developers.arcgis.com/web-map-specification/objects/labelingInfo/
            // This particular JSON string will have the following characteristics:
            string redLabelJson =
             @"{
                    ""labelExpressionInfo"":{""expression"":""$feature.NAME + ' (' + left($feature.PARTY,1) + ')\\nDistrict' + $feature.CDFIPS""},
                    ""labelPlacement"":""esriServerPolygonPlacementAlwaysHorizontal"",
                    ""where"":""PARTY = 'Republican'"",
                    ""symbol"":
                        { 
                            ""angle"":0,
                            ""backgroundColor"":[0,0,0,0],
                            ""borderLineColor"":[0,0,0,0],
                            ""borderLineSize"":0,
                            ""color"":[255,0,0,255],
                            ""font"":
                                {
                                    ""decoration"":""none"",
                                    ""size"":10,
                                    ""style"":""normal"",
                                    ""weight"":""normal""
                                },
                            ""haloColor"":[255,255,255,255],
                            ""haloSize"":2,
                            ""horizontalAlignment"":""center"",
                            ""kerning"":false,
                            ""type"":""esriTS"",
                            ""verticalAlignment"":""middle"",
                            ""xoffset"":0,
                            ""yoffset"":0
                        }
               }";

            string blueLabelJson =
                @"{
                    ""labelExpressionInfo"":{""expression"":""$feature.NAME + ' (' + left($feature.PARTY,1) + ')\\nDistrict' + $feature.CDFIPS""},
                    ""labelPlacement"":""esriServerPolygonPlacementAlwaysHorizontal"",
                    ""where"":""PARTY = 'Democrat'"",
                    ""symbol"":
                        { 
                            ""angle"":0,
                            ""backgroundColor"":[0,0,0,0],
                            ""borderLineColor"":[0,0,0,0],
                            ""borderLineSize"":0,
                            ""color"":[0,0,255,255],
                            ""font"":
                                {
                                    ""decoration"":""none"",
                                    ""size"":10,
                                    ""style"":""normal"",
                                    ""weight"":""normal""
                                },
                            ""haloColor"":[255,255,255,255],
                            ""haloSize"":2,
                            ""horizontalAlignment"":""center"",
                            ""kerning"":false,
                            ""type"":""esriTS"",
                            ""verticalAlignment"":""middle"",
                            ""xoffset"":0,
                            ""yoffset"":0
                        }
               }";

            // Create a label definition from the JSON string. 
            LabelDefinition redLabelDefinition = LabelDefinition.FromJson(redLabelJson);
            LabelDefinition blueLabelDefinition = LabelDefinition.FromJson(blueLabelJson);

            // Add the label definition to the feature layer's label definition collection.
            districtFeatureLabel.LabelDefinitions.Add(redLabelDefinition);
            districtFeatureLabel.LabelDefinitions.Add(blueLabelDefinition);

            // Enable the visibility of labels to be seen.
            districtFeatureLabel.LabelsEnabled = true;
        }

    }
}
