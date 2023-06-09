using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FFXIVLooseTextureCompiler.Racial {
    public static class RacePaths {
        private static bool otopopNotice;
        public static event EventHandler otopopNoticeTriggered;
        public static string VersionText { get; set; }

        public static string GetFaceTexturePath(int material, int gender, int subRaceValue, int facePart, int faceType, int auraFaceScales, bool asym) {
            string selectedText = RaceInfo.SubRaces[subRaceValue];
            if (facePart == 2 && asym) {
                string faceIdCheck = "00";
                if (selectedText.ToLower() == "the lost" || selectedText.ToLower() == "hellsgaurd" || selectedText.ToLower() == "highlander"
                    || selectedText.ToLower() == "duskwight" || selectedText.ToLower() == "keeper" || selectedText.ToLower() == "dunesfolk"
                    || (selectedText.ToLower() == "xaela") || (selectedText.ToLower() == "veena")) {
                    faceIdCheck = "10";
                }
                return "chara/asymeyes/" + RaceInfo.Races[RaceInfo.SubRaceToMainRace(subRaceValue)]
                    .ToLower().Replace("midlander", "hyur").Replace("highlander", "hyur")
                    .ToLower().Replace("xaela", "aura").Replace("raen", "aura")
                    .Replace(@"'", null) + "_"
                    + RaceInfo.SubRaces[subRaceValue].Replace(" ", null).ToLower()
                    + "_" + (gender == 0 ? "male" : "female") + "/f" + faceIdCheck + (faceType + 1)
                    + GetTextureType(material, 0, false, true) + ".tex";
            }
            if (material != 3) {
                string faceIdCheck = "000";
                if (selectedText.ToLower() == "the lost" || selectedText.ToLower() == "hellsguard" || selectedText.ToLower() == "highlander"
                    || selectedText.ToLower() == "duskwight" || selectedText.ToLower() == "keeper" || selectedText.ToLower() == "dunesfolk"
                    || (selectedText.ToLower() == "xaela" && facePart != 2 && (material == 0 || auraFaceScales == 2))
                    || (selectedText.ToLower() == "veena" && facePart == 1 && material != 2)
                    || (selectedText.ToLower() == "veena" && facePart == 2 && material == 2)) {
                    faceIdCheck = "010";
                }
                string subRace = (gender == 0 ? RaceInfo.RaceCodeFace.Masculine[subRaceValue]
                    : RaceInfo.RaceCodeFace.Feminine[subRaceValue]);
                int faceOffset = (faceType + (subRaceValue == 12 || subRaceValue == 13 ? 4 : 0)) + 1;
                return "chara/human/c" + subRace + "/obj/face/f" + faceIdCheck + faceOffset + "/texture/--c"
                    + subRace + "f" + faceIdCheck + faceOffset
                    + GetFacePart(facePart, asym) + GetTextureType(material, 0, true) + ".tex";
            } else {
                return "chara/common/texture/catchlight_1.tex";
            }
        }
        public static string GetBodyTexturePath(int texture, int genderValue, int baseBody, int race, int tail, bool uniqueAuRa = false) {
            string result = "";
            string unique = RaceInfo.Races[race].Contains("Xaela") ? "0101" : "0001";
            switch (baseBody) {
                case 0:
                    // Vanila
                    if (texture == 2 && race == 5) {
                        result = @"chara/common/texture/skin_m.tex";
                    } else {
                        string genderCode = (genderValue == 0 ? RaceInfo.RaceCodeBody.Masculine[race]
                            : RaceInfo.RaceCodeBody.Feminine[race]);
                        result = @"chara/human/c" + genderCode + @"/obj/body/b" + unique
                            + @"/texture/--c" + genderCode + "b" + unique + GetTextureType(texture, baseBody) + ".tex";
                    }
                    break;
                case 1:
                    // Bibo+
                    if (race != 5) {
                        if (genderValue == 1) {
                            result = @"chara/bibo/" + RaceInfo.BodyIdentifiers[baseBody].RaceIdentifiers[race]
                                + GetTextureType(texture, baseBody) + ".tex";
                        } else {
                            result = "";
                        }
                    } else {
                        result = "Bibo+ is not compatible with lalafells";
                    }
                    break;
                case 2:
                    // Eve
                    if (race != 5) {
                        if (genderValue == 1) {
                            if (texture != 2) {
                                result = @"chara/human/c" + (genderValue == 0 ? RaceInfo.RaceCodeBody.Masculine[race]
                                    : RaceInfo.RaceCodeBody.Feminine[race]) + @"/obj/body/b" + "0001" + @"/texture/eve2" +
                                    RaceInfo.BodyIdentifiers[baseBody].RaceIdentifiers[race] + GetTextureType(texture, baseBody) + ".tex";
                            } else {
                                if (race == 6) {
                                    result = "chara/human/c1401/obj/body/b0001/texture/eve2lizard_m.tex";
                                } else if (race == 7) {
                                    result = "chara/human/c1401/obj/body/b0001/texture/eve2lizard2_m.tex";
                                } else {
                                    result = "chara/common/texture/skin_gen3.tex";
                                }
                            }
                        } else {
                            result = "Eve is only compatible with feminine characters";
                        }
                    } else {
                        result = "Eve is not compatible with lalafells";
                    }
                    break;
                case 3:
                    // Gen3 and T&F3
                    if (race != 5) {
                        if (genderValue == 1) {
                            result = @"chara/human/c" + (genderValue == 0 ? RaceInfo.RaceCodeBody.Masculine[race]
                                : RaceInfo.RaceCodeBody.Feminine[race]) + @"/obj/body/b" + unique + @"/texture/tfgen3" +
                                RaceInfo.BodyIdentifiers[baseBody].RaceIdentifiers[race] + "f" + GetTextureType(texture, baseBody) + ".tex";
                        } else {
                            result = "Gen3 and T&F3 are only compatible with feminine characters";
                        }
                    } else {
                        result = "Gen3 and T&F3 are not compatible with lalafells";
                    }
                    break;
                case 4:
                    // Scales+
                    if (race != 5) {
                        if (race == 6 || race == 7) {
                            if (genderValue == 1) {
                                result = @"chara/bibo/" + RaceInfo.BodyIdentifiers[baseBody].RaceIdentifiers[race] +
                                    GetTextureType(texture, baseBody) + ".tex";
                            } else {
                                result = "Scales+ is only compatible with feminine Au Ra characters";
                            }
                        } else {
                            result = "Scales+ is only compatible with feminine Au Ra characters";
                        }
                    } else {
                        result = "Scales+ is not compatible with lalafells";
                    }
                    break;
                case 5:
                    if (race != 5) {
                        if (genderValue == 0) {
                            // TBSE and HRBODY
                            if (texture == 1 || texture == 2) {
                                unique = uniqueAuRa ? "0101" : "0001";
                            }
                            result = @"chara/human/c" + (genderValue == 0 ? RaceInfo.RaceCodeBody.Masculine[race]
                                : RaceInfo.RaceCodeBody.Feminine[race]) + @"/obj/body/b" + unique
                                + @"/texture/--c" + RaceInfo.RaceCodeBody.Masculine[race] + "b" + unique + "_b" + GetTextureType(texture, baseBody) + ".tex";
                        } else {
                            result = "TBSE and HRBODY are only compatible with masculine characters";
                        }
                    } else {
                        result = "TBSE and HRBODY are not compatible with lalafells";
                    }
                    break;
                case 6:
                    // Tails
                    string xaelaCheck = (race == 7 ? "010" : "000") + (tail + 1);
                    string gender = (genderValue == 0 ? RaceInfo.RaceCodeBody.Masculine[race]
                        : RaceInfo.RaceCodeBody.Feminine[race]);
                    result = @"chara/human/c" + gender + @"/obj/tail/t" + xaelaCheck + @"/texture/--c" + gender + "t" +
                        xaelaCheck + "_etc" + GetTextureType(texture, baseBody) + ".tex";
                    break;
                case 7:
                    // Otopop
                    if (race == 5) {
                        if (texture == 0) {
                            if (!otopopNotice) {
                                if (otopopNoticeTriggered != null) {
                                    otopopNoticeTriggered.Invoke(new object(), EventArgs.Empty);
                                }
                                otopopNotice = true;
                            }
                        }
                        result = @"chara/human/c1101/obj/body/b0001/texture/v01_c1101b0001_g" + GetTextureType(texture, baseBody) + ".tex";

                    } else {
                        result = "Otopop is only compatible with lalafells";
                    }
                    break;
                case 8:
                    // Asymmetrical Vanilla Lalafell
                    if (race == 5) {
                        result = @"chara/human/c1101/obj/body/b0001/texture/v01_c1101b0001_b" + GetTextureType(texture, baseBody) + ".tex";
                    } else {
                        result = "Asymmetrical Vanilla Lalafell is only compatible with lalafells";
                    }
                    break;
            }
            return result;
        }
        public static string GetHairTexturePath(int material, int hairNumber, int gender, int race, int subRaceValue) {
            string hairValue = RaceInfo.NumberPadder(hairNumber + 1);
            string genderCode = (gender == 0 ? RaceInfo.RaceCodeBody.Masculine[race]
                : RaceInfo.RaceCodeBody.Feminine[race]);
            string subRace = (gender == 0 ? RaceInfo.RaceCodeFace.Masculine[subRaceValue]
                : RaceInfo.RaceCodeFace.Feminine[subRaceValue]);
            return "chara/human/c" + genderCode + "/obj/hair/h" + hairValue + "/texture/--c"
                + genderCode + "h" + hairValue + "_hir" + GetTextureType(material, 0, true) + ".tex";
        }
        public static string GetTextureType(int material, int baseBodyIndex, bool isface = false, bool isVerbose = false) {
            switch (material) {
                case 0:
                    return isVerbose ? "_diffuse" : "_d";
                case 1:
                    return isVerbose ? "_normal" : "_n";
                case 2:
                    if (baseBodyIndex == 1 && !isface) {
                        return isVerbose ? "_multi" : "_m";
                    } else {
                        return isVerbose ? "_multi" : "_s";
                    }
                case 3:
                    return "_catchlight";
            }
            return null;
        }
        public static string GetFacePart(int material, bool asym) {
            switch (material) {
                case 0:
                    return asym ? "_fac_b" : "_fac";
                case 1:
                case 3:
                    return "_etc";
                case 2:
                    return "_iri";
                case 6:
                    return "_fac_b";
                case 7:
                    return "_etc_b";
            }
            return null;
        }
    }
}
