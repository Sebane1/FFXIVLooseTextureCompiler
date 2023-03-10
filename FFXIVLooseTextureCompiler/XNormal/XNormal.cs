using FFXIVLooseTextureCompiler.PathOrganization;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FFXIVLooseTextureCompiler {
    public class XNormal {
        private static string xmlFile = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\r\n<Settings xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" Version=\"3.19.3\">\r\n  <HighPolyModel DefaultMeshScale=\"1.000000\">\r\n    <Mesh Visible=\"true\" Scale=\"1.000000\" IgnorePerVertexColor=\"true\" AverageNormals=\"UseExportedNormals\" BaseTexIsTSNM=\"false\" File=\"{0}\" PositionOffset=\"0.0000;0.0000;0.0000\" BaseTex=\"{1}\"/>\r\n    <Mesh Visible=\"false\" Scale=\"1.000000\" IgnorePerVertexColor=\"true\" AverageNormals=\"UseExportedNormals\" BaseTexIsTSNM=\"false\" File=\"{2}\" PositionOffset=\"0.0000;0.0000;0.0000\"/>\r\n  </HighPolyModel>\r\n  <LowPolyModel DefaultMeshScale=\"1.000000\">\r\n    <Mesh Visible=\"true\" File=\"{2}\" AverageNormals=\"UseExportedNormals\" MaxRayDistanceFront=\"0.500000\" MaxRayDistanceBack=\"0.500000\" UseCage=\"false\" NormapMapType=\"Tangent-space\" UsePerVertexColors=\"true\" UseFresnel=\"false\" FresnelRefractiveIndex=\"1.330000\" ReflectHDRMult=\"1.000000\" VectorDisplacementTS=\"false\" VDMSwizzleX=\"X+\" VDMSwizzleY=\"Y+\" VDMSwizzleZ=\"Z+\" BatchProtect=\"false\" CastShadows=\"true\" ReceiveShadows=\"true\" BackfaceCull=\"true\" NMSwizzleX=\"X+\" NMSwizzleY=\"Y+\" NMSwizzleZ=\"Z+\" HighpolyNormalsOverrideTangentSpace=\"true\" TransparencyMode=\"None\" AlphaTestValue=\"127\" Matte=\"false\" Scale=\"1.000000\" MatchUVs=\"false\" UOffset=\"0.000000\" VOffset=\"0.000000\" PositionOffset=\"0.0000;0.0000;0.0000\"/>\r\n  </LowPolyModel>\r\n  <GenerateMaps GenNormals=\"false\" Width=\"4096\" Height=\"4096\" EdgePadding=\"16\" BucketSize=\"32\" TangentSpace=\"true\" ClosestIfFails=\"true\" DiscardRayBackFacesHits=\"true\" File=\"{3}\" SwizzleX=\"X+\" SwizzleY=\"Y+\" SwizzleZ=\"Z+\" AA=\"1\" BakeHighpolyBaseTex=\"true\" BakeHighpolyBaseTextureDrawObjectIDIfNoTexture=\"false\" GenHeights=\"false\" HeightTonemap=\"Interactive\" HeightTonemapMin=\"-100.000000\" HeightTonemapMax=\"100.000000\" GenAO=\"false\" AORaysPerSample=\"128\" AODistribution=\"Uniform\" AOConeAngle=\"162.000000\" AOBias=\"0.080000\" AOAllowPureOccluded=\"true\" AOLimitRayDistance=\"false\" AOAttenConstant=\"1.000000\" AOAttenLinear=\"0.000000\" AOAttenCuadratic=\"0.000000\" AOJitter=\"false\" AOIgnoreBackfaceHits=\"false\" GenBent=\"false\" BentRaysPerSample=\"128\" BentConeAngle=\"162.000000\" BentBias=\"0.080000\" BentTangentSpace=\"false\" BentLimitRayDistance=\"false\" BentJitter=\"false\" BentDistribution=\"Uniform\" BentSwizzleX=\"X+\" BentSwizzleY=\"Y+\" BentSwizzleZ=\"Z+\" GenPRT=\"false\" PRTRaysPerSample=\"128\" PRTConeAngle=\"179.500000\" PRTBias=\"0.080000\" PRTLimitRayDistance=\"false\" PRTJitter=\"false\" PRTNormalize=\"true\" PRTThreshold=\"0.005000\" GenProximity=\"false\" ProximityRaysPerSample=\"128\" ProximityConeAngle=\"80.000000\" ProximityLimitRayDistance=\"true\" ProximityFlipNormals=\"false\" ProximityFlipValue=\"false\" GenConvexity=\"false\" ConvexityScale=\"1.000000\" GenThickness=\"false\" GenCavity=\"false\" CavityRaysPerSample=\"128\" CavityJitter=\"false\" CavitySearchRadius=\"0.500000\" CavityContrast=\"1.250000\" CavitySteps=\"4\" GenWireRays=\"false\" RenderRayFails=\"true\" RenderWireframe=\"true\" GenDirections=\"false\" DirectionsTS=\"false\" DirectionsSwizzleX=\"X+\" DirectionsSwizzleY=\"Y+\" DirectionsSwizzleZ=\"Z+\" DirectionsTonemap=\"Interactive\" DirectionsTonemapMin=\"false\" DirectionsTonemapMax=\"false\" GenRadiosityNormals=\"false\" RadiosityNormalsRaysPerSample=\"128\" RadiosityNormalsDistribution=\"Uniform\" RadiosityNormalsConeAngle=\"162.000000\" RadiosityNormalsBias=\"0.080000\" RadiosityNormalsLimitRayDistance=\"false\" RadiosityNormalsAttenConstant=\"1.000000\" RadiosityNormalsAttenLinear=\"0.000000\" RadiosityNormalsAttenCuadratic=\"0.000000\" RadiosityNormalsJitter=\"false\" RadiosityNormalsContrast=\"1.000000\" RadiosityNormalsEncodeAO=\"true\" RadiosityNormalsCoordSys=\"AliB\" RadiosityNormalsAllowPureOcclusion=\"false\" BakeHighpolyVCols=\"false\" GenCurv=\"false\" CurvRaysPerSample=\"128\" CurvBias=\"0.000100\" CurvConeAngle=\"162.000000\" CurvJitter=\"false\" CurvSearchDistance=\"1.000000\" CurvTonemap=\"3Col\" CurvDistribution=\"Cosine\" CurvAlgorithm=\"Average\" CurvSmoothing=\"true\" GenDerivNM=\"false\" GenTranslu=\"false\" TransluRaysPerSample=\"128\" TransluDistribution=\"Uniform\" TransluConeAngle=\"162.000000\" TransluBias=\"0.000500\" TransluDist=\"1.000000\" TransluJitter=\"false\">\r\n    <NMBackgroundColor R=\"128\" G=\"128\" B=\"255\"/>\r\n    <BakeHighpolyBaseTextureNoTexCol R=\"255\" G=\"0\" B=\"0\"/>\r\n    <BakeHighpolyBaseTextureBackgroundColor R=\"0\" G=\"0\" B=\"0\"/>\r\n    <HMBackgroundColor R=\"0\" G=\"0\" B=\"0\"/>\r\n    <AOOccludedColor R=\"0\" G=\"0\" B=\"0\"/>\r\n    <AOUnoccludedColor R=\"255\" G=\"255\" B=\"255\"/>\r\n    <AOBackgroundColor R=\"255\" G=\"255\" B=\"255\"/>\r\n    <BentBackgroundColor R=\"127\" G=\"127\" B=\"255\"/>\r\n    <PRTBackgroundColor R=\"0\" G=\"0\" B=\"0\"/>\r\n    <ProximityBackgroundColor R=\"255\" G=\"255\" B=\"255\"/>\r\n    <ConvexityBackgroundColor R=\"255\" G=\"255\" B=\"255\"/>\r\n    <CavityBackgroundColor R=\"255\" G=\"255\" B=\"255\"/>\r\n    <RenderWireframeCol R=\"255\" G=\"255\" B=\"255\"/>\r\n    <RenderCWCol R=\"0\" G=\"0\" B=\"255\"/>\r\n    <RenderSeamCol R=\"0\" G=\"255\" B=\"0\"/>\r\n    <RenderRayFailsCol R=\"255\" G=\"0\" B=\"0\"/>\r\n    <RenderWireframeBackgroundColor R=\"0\" G=\"0\" B=\"0\"/>\r\n    <VDMBackgroundColor R=\"0\" G=\"0\" B=\"0\"/>\r\n    <RadNMBackgroundColor R=\"0\" G=\"0\" B=\"0\"/>\r\n    <BakeHighpolyVColsBackgroundCol R=\"255\" G=\"255\" B=\"255\"/>\r\n    <CurvBackgroundColor R=\"0\" G=\"0\" B=\"0\"/>\r\n    <DerivNMBackgroundColor R=\"127\" G=\"127\" B=\"0\"/>\r\n    <TransluBackgroundColor R=\"0\" G=\"0\" B=\"0\"/>\r\n  </GenerateMaps>\r\n  <Detail Scale=\"0.500000\" Method=\"4Samples\"/>\r\n  <Viewer3D ShowGrid=\"true\" ShowWireframe=\"false\" ShowTangents=\"false\" ShowNormals=\"false\" ShowBlockers=\"false\" MaxTessellationLevel=\"0\" LightIntensity=\"1.000000\" LightIndirectIntensity=\"0.000000\" Exposure=\"0.180000\" HDRThreshold=\"0.900000\" UseGlow=\"true\" GlowIntensity=\"1.000000\" SSAOEnabled=\"false\" SSAOBright=\"1.100000\" SSAOContrast=\"1.000000\" SSAOAtten=\"1.000000\" SSAORadius=\"0.250000\" SSAOBlurRadius=\"2.000000\" ParallaxStrength=\"0.000000\" ShowHighpolys=\"true\" ShowAO=\"false\" CageOpacity=\"0.700000\" DiffuseGIIntensity=\"1.000000\" CastShadows=\"false\" ShadowBias=\"0.100000\" ShadowArea=\"0.250000\" AxisScl=\"0.040000\" CameraOrbitDistance=\"0.500000\" CameraOrbitAutoCenter=\"true\" ShowStarfield=\"false\">\r\n    <LightAmbientColor R=\"33\" G=\"33\" B=\"33\"/>\r\n    <LightDiffuseColor R=\"229\" G=\"229\" B=\"229\"/>\r\n    <LightSpecularColor R=\"255\" G=\"255\" B=\"255\"/>\r\n    <LightSecondaryColor R=\"0\" G=\"0\" B=\"0\"/>\r\n    <LightTertiaryColor R=\"0\" G=\"0\" B=\"0\"/>\r\n    <BackgroundColor R=\"0\" G=\"0\" B=\"0\"/>\r\n    <GridColor R=\"180\" G=\"180\" B=\"220\"/>\r\n    <CageColor R=\"76\" G=\"76\" B=\"76\"/>\r\n    <CameraRotation e11=\"1.000000\" e12=\"0.000000\" e13=\"0.000000\" e21=\"0.000000\" e22=\"1.000000\" e23=\"0.000000\" e31=\"0.000000\" e32=\"0.000000\" e33=\"1.000000\"/>\r\n    <CameraPosition x=\"0.000000\" y=\"1.000000\" z=\"0.000000\"/>\r\n    <LightPosition x=\"0.000000\" y=\"2.000000\" z=\"5.000000\"/>\r\n  </Viewer3D>\r\n</Settings>\r\n";
        private const string xNormal = "res\\xnormal\\x64\\xNormal.exe";
        private const string bibo = "res\\model\\bplus.FBX";
        private const string gen2 = "res\\model\\gen2.FBX";
        private const string gen3 = "res\\model\\gen3.FBX";
        private const string vanillaLala = "res\\model\\vanillap.fbx";
        private const string otopop = "res\\model\\genp.fbx";
        private const string redefinedLala = "res\\model\\redefinedLala.fbx";
        int count = 0;

        List<XNormalExportJob> biboToGen2Batch = new List<XNormalExportJob>();
        List<XNormalExportJob> biboToGen3Batch = new List<XNormalExportJob>();
        List<XNormalExportJob> gen3ToGen2Batch = new List<XNormalExportJob>();
        List<XNormalExportJob> gen3ToBiboBatch = new List<XNormalExportJob>();
        List<XNormalExportJob> gen2ToBiboBatch = new List<XNormalExportJob>();
        List<XNormalExportJob> gen2ToGen3Batch = new List<XNormalExportJob>();

        List<XNormalExportJob> otopopToVanillaLalaBatch = new List<XNormalExportJob>();
        List<XNormalExportJob> otopopToRedefinedLalaBatch = new List<XNormalExportJob>();
        List<XNormalExportJob> redefinedLalaToVanillaBatch = new List<XNormalExportJob>();
        List<XNormalExportJob> redefinedLalaToOtopopBatch = new List<XNormalExportJob>();
        List<XNormalExportJob> vanillaLalaToOtopopBatch = new List<XNormalExportJob>();
        List<XNormalExportJob> vanillaLalaToRedefinedLalaBatch = new List<XNormalExportJob>();


        private static string xmlFileName = "template.xml";

        public static string XmlFileName { get => xmlFileName; set => xmlFileName = value; }

        public static void GenerateBasedOnSourceBody(string internalPath, string inputPath, string outputPath) {
            if (internalPath.Contains("bibo")) {
                if (outputPath.Contains("gen2")) {
                    BiboToGen2(inputPath, outputPath);
                }
                if (outputPath.Contains("gen3")) {
                    BiboToGen3(inputPath, outputPath);
                }
            } else if (internalPath.Contains("eve") || internalPath.Contains("gen3")) {
                if (outputPath.Contains("gen2")) {
                    Gen3ToGen2(inputPath, outputPath);
                }
                if (outputPath.Contains("bibo")) {
                    Gen3ToBibo(inputPath, outputPath);
                }
            } else if (internalPath.Contains("body")) {
                if (outputPath.Contains("bibo")) {
                    Gen2ToBibo(inputPath, outputPath);
                }
                if (outputPath.Contains($"gen3")) {
                    Gen2ToGen3(inputPath, outputPath);
                }
            } else if (internalPath.Contains("skin_otopop") || internalPath.Contains("v01_c1101b0001_g")) {
                if (outputPath.Contains("redefined_lala")) {
                    OtopopToRedefinedLala(inputPath, outputPath);
                }
                if (outputPath.Contains("vanilla_lala")) {
                    OtopopToVanillaLala(inputPath, outputPath);
                }
            } else if (internalPath.Contains("v01_c1101b0001_b")) {
                if (outputPath.Contains("otopop")) {
                    RedefinedLalaToOtopop(inputPath, outputPath);
                }
                if (outputPath.Contains("vanilla_lala")) {
                    RedefinedLalaToVanillaLala(inputPath, outputPath);
                }
            } else if (internalPath.Contains("v01_c1101b0001") || internalPath.Contains("--c1101b0001")) {
                if (outputPath.Contains("redefined_lala")) {
                    VanillaLalaToRedefinedLala(inputPath, outputPath);
                }
                if (outputPath.Contains("otopop")) {
                    VanillaLalaToOtopop(inputPath, outputPath);
                }
            }
        }
        public void AddToBatch(string internalPath, string inputPath, string outputPath) {
            if (internalPath.Contains("bibo")) {
                if (outputPath.Contains("gen2")) {
                    biboToGen2Batch.Add(new XNormalExportJob(internalPath, inputPath, outputPath, bibo, gen2, count++ + ".xml"));
                }
                if (outputPath.Contains("gen3")) {
                    biboToGen3Batch.Add(new XNormalExportJob(internalPath, inputPath, outputPath, bibo, gen3, count++ + ".xml"));
                }
            } else if (internalPath.Contains("eve") || internalPath.Contains("gen3")) {
                if (outputPath.Contains("gen2")) {
                    gen3ToGen2Batch.Add(new XNormalExportJob(internalPath, inputPath, outputPath, gen3, gen2, count++ + ".xml"));
                }
                if (outputPath.Contains("bibo")) {
                    gen3ToBiboBatch.Add(new XNormalExportJob(internalPath, inputPath, outputPath, gen3, bibo, count++ + ".xml"));
                }
            } else if (internalPath.Contains("body")) {
                if (outputPath.Contains("bibo")) {
                    gen2ToBiboBatch.Add(new XNormalExportJob(internalPath, inputPath, outputPath, gen2, bibo, count++ + ".xml"));
                }
                if (outputPath.Contains($"gen3")) {
                    gen2ToGen3Batch.Add(new XNormalExportJob(internalPath, inputPath, outputPath, gen2, gen3, count++ + ".xml"));
                }
            } else if (internalPath.Contains("skin_otopop") || internalPath.Contains("v01_c1101b0001_g")) {
                if (outputPath.Contains("redefined_lala")) {
                    otopopToRedefinedLalaBatch.Add(new XNormalExportJob(internalPath, inputPath, outputPath, otopop, redefinedLala, count++ + ".xml"));
                }
                if (outputPath.Contains("vanilla_lala")) {
                    otopopToVanillaLalaBatch.Add(new XNormalExportJob(internalPath, inputPath, outputPath, otopop, vanillaLala, count++ + ".xml"));
                }
            } else if (internalPath.Contains("v01_c1101b0001_b")) {
                if (outputPath.Contains("otopop")) {
                    redefinedLalaToOtopopBatch.Add(new XNormalExportJob(internalPath, inputPath, outputPath, redefinedLala, otopop, count++ + ".xml"));
                }
                if (outputPath.Contains("vanilla_lala")) {
                    redefinedLalaToVanillaBatch.Add(new XNormalExportJob(internalPath, inputPath, outputPath, redefinedLala, vanillaLala, count++ + ".xml"));
                }
            } else if (internalPath.Contains("v01_c1101b0001") || internalPath.Contains("--c1101b0001")) {
                if (outputPath.Contains("redefined_lala")) {
                    vanillaLalaToRedefinedLalaBatch.Add(new XNormalExportJob(internalPath, inputPath, outputPath, vanillaLala, redefinedLala, count++ + ".xml"));
                }
                if (outputPath.Contains("otopop")) {
                    vanillaLalaToOtopopBatch.Add(new XNormalExportJob(internalPath, inputPath, outputPath, vanillaLala, otopop, count++ + ".xml"));
                }
            }
        }
        public static void BiboToGen2(string inputImage, string outputImage) {
            CallXNormal(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, bibo),
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, gen2), inputImage, outputImage.Replace("_baseTexBaked", null));
        }
        public static void BiboToGen3(string inputImage, string outputImage) {
            CallXNormal(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, bibo),
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, gen3), inputImage, outputImage.Replace("_baseTexBaked", null));
        }
        public static void Gen3ToGen2(string inputImage, string outputImage) {
            CallXNormal(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, gen3),
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, gen2), inputImage, outputImage.Replace("_baseTexBaked", null));
        }
        public static void Gen3ToBibo(string inputImage, string outputImage) {
            CallXNormal(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, gen3),
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, bibo), inputImage, outputImage.Replace("_baseTexBaked", null));
        }
        public static void Gen2ToBibo(string inputImage, string outputImage) {
            CallXNormal(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, gen2),
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, bibo), inputImage, outputImage.Replace("_baseTexBaked", null));
        }
        public static void Gen2ToGen3(string inputImage, string outputImage) {
            CallXNormal(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, gen2),
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, gen3), inputImage, outputImage.Replace("_baseTexBaked", null));
        }

        public static void OtopopToVanillaLala(string inputImage, string outputImage) {
            CallXNormal(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, otopop),
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, vanillaLala), inputImage, outputImage.Replace("_baseTexBaked", null));
        }

        public static void OtopopToRedefinedLala(string inputImage, string outputImage) {
            CallXNormal(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, otopop),
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, redefinedLala), inputImage, outputImage.Replace("_baseTexBaked", null));
        }
        public static void RedefinedLalaToOtopop(string inputImage, string outputImage) {
            CallXNormal(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, redefinedLala),
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, otopop), inputImage, outputImage.Replace("_baseTexBaked", null));
        }
        public static void RedefinedLalaToVanillaLala(string inputImage, string outputImage) {
            CallXNormal(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, redefinedLala),
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, vanillaLala), inputImage, outputImage.Replace("_baseTexBaked", null));
        }
        public static void VanillaLalaToOtopop(string inputImage, string outputImage) {
            CallXNormal(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, vanillaLala),
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, otopop), inputImage, outputImage.Replace("_baseTexBaked", null));
        }

        public static void VanillaLalaToRedefinedLala(string inputImage, string outputImage) {
            CallXNormal(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, vanillaLala),
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, redefinedLala), inputImage, outputImage.Replace("_baseTexBaked", null));
        }

        public static void OpenXNormal() {
            string executable = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, xNormal);
            Process process = Process.Start(executable);
        }

        public static void CallXNormal(string inputFBX, string outputFBX, string inputImage, string outputImage) {
            string path = Path.Combine(Application.UserAppDataPath, xmlFileName);
            string executable = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, xNormal);
            if (!Directory.Exists(Application.UserAppDataPath)) {
                Directory.CreateDirectory(Application.UserAppDataPath);
            }
            using (StreamWriter writer = new StreamWriter(path)) {
                writer.Write(string.Format(xmlFile,
                    CleanXmlEscapeSequences(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, inputFBX)),
                    CleanXmlEscapeSequences(inputImage),
                    CleanXmlEscapeSequences(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, outputFBX)),
                    CleanXmlEscapeSequences(outputImage)));
            }
            ProcessStartInfo processStartInfo = new ProcessStartInfo(@"""" + executable + @"""");
            processStartInfo.UseShellExecute = true;
            processStartInfo.Arguments = @"""" + path + @"""";
            Process process = Process.Start(processStartInfo);
            process.WaitForExit();
            Thread.Sleep(100);
        }
        public void ProcessBatches() {
            List<XNormalExportJob> exportJobs = new List<XNormalExportJob>();
            exportJobs.AddRange(biboToGen2Batch);
            exportJobs.AddRange(biboToGen3Batch);
            exportJobs.AddRange(gen3ToBiboBatch);
            exportJobs.AddRange(gen3ToGen2Batch);
            exportJobs.AddRange(gen2ToGen3Batch);
            exportJobs.AddRange(gen2ToGen3Batch);

            exportJobs.AddRange(otopopToRedefinedLalaBatch);
            exportJobs.AddRange(otopopToVanillaLalaBatch);
            exportJobs.AddRange(redefinedLalaToOtopopBatch);
            exportJobs.AddRange(redefinedLalaToOtopopBatch);
            exportJobs.AddRange(vanillaLalaToOtopopBatch);
            exportJobs.AddRange(vanillaLalaToRedefinedLalaBatch);

            string executable = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, xNormal);
            ProcessStartInfo processStartInfo = new ProcessStartInfo(@"""" + executable + @"""");
            processStartInfo.UseShellExecute = true;
            processStartInfo.Arguments = "";
            if (!Directory.Exists(Application.UserAppDataPath)) {
                Directory.CreateDirectory(Application.UserAppDataPath);
            }
            foreach (XNormalExportJob xNormalExportJob in exportJobs) {
                string path = Path.Combine(Application.UserAppDataPath, xNormalExportJob.OutputXMLPath);
                processStartInfo.Arguments += @"""" + path + @""" ";
                using (StreamWriter writer = new StreamWriter(path)) {
                    writer.Write(string.Format(xmlFile,
                    CleanXmlEscapeSequences(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, xNormalExportJob.InputModel)),
                    CleanXmlEscapeSequences(xNormalExportJob.InputTexturePath),
                    CleanXmlEscapeSequences(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, xNormalExportJob.OutputModel)),
                    CleanXmlEscapeSequences(xNormalExportJob.OutputTexturePath.Replace("_baseTexBaked", null))));
                }
            }
            if (exportJobs.Count > 0) {
                processStartInfo.Arguments = processStartInfo.Arguments.Trim();
                Process process = Process.Start(processStartInfo);
                process.WaitForExit();

                foreach (XNormalExportJob xNormalExportJob in exportJobs) {
                    string path = Path.Combine(Application.UserAppDataPath, xNormalExportJob.OutputXMLPath);
                    if (!File.Exists(xNormalExportJob.OutputTexturePath)) {
                        CallXNormal(xNormalExportJob.InputModel, xNormalExportJob.OutputModel,
                            xNormalExportJob.InputTexturePath, xNormalExportJob.OutputTexturePath);
                    }
                }
            }
        }
        public static string CleanXmlEscapeSequences(string input) {
            return input.Replace("&", @"&#38;").Replace("'", @"&#39;").Replace("'", @"&#39;")
                .Replace("<", @"&#60;").Replace(">", @"&#62;").Replace(@"""", @"&#34");
        }
    }
}
