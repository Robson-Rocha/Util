﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using RobsonROX.Util.Dictionaries;

namespace RobsonROX.Util.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public class EnglishPluralizationService
    {
        // ReSharper disable once UnusedMethodReturnValue.Local
        private static T CheckArgumentNull<T>(T value, string parameterName) where T : class
        {
            if (value == null) throw new ArgumentNullException(parameterName);
            return value;
        }

        private static bool DoesWordContainSuffix(string word, IEnumerable<string> suffixes, CultureInfo culture)
        {
            return suffixes.Any(s => word.EndsWith(s, true, culture));
        }

        private static bool TryInflectOnSuffixInWord(string word, IEnumerable<string> suffixes, Func<string, string> operationOnWord, CultureInfo culture, out string newWord)
        {
            newWord = null;

            if (DoesWordContainSuffix(word, suffixes, culture))
            {
                newWord = operationOnWord(word);
                return true;
            }
            return false;
        } 


        /// <summary>
        /// 
        /// </summary>
        private CultureInfo Culture { get; }

        private readonly BidirectionalDictionary<string, string> _userDictionary;
        private readonly StringBidirectionalDictionary _irregularPluralsPluralizationService;
        private readonly StringBidirectionalDictionary _assimilatedClassicalInflectionPluralizationService;
        private readonly StringBidirectionalDictionary _oSuffixPluralizationService;
        private readonly StringBidirectionalDictionary _classicalInflectionPluralizationService;
        private readonly StringBidirectionalDictionary _irregularVerbPluralizationService;
        private readonly StringBidirectionalDictionary _wordsEndingWithSePluralizationService;
        private readonly StringBidirectionalDictionary _wordsEndingWithSisPluralizationService;
        private readonly StringBidirectionalDictionary _wordsEndingWithSusPluralizationService;
        private readonly StringBidirectionalDictionary _wordsEndingWithInxAnxYnxPluralizationService;

        private readonly List<string> _knownSingluarWords;
        private readonly List<string> _knownPluralWords;

        private readonly string[] _uninflectiveSuffixList = { "fish", "ois", "sheep", "deer", "pos", "itis", "ism" };

        private readonly string[] _uninflectiveWordList =
        {
            "bison", "flounder", "pliers", "bream", "gallows", "proceedings", 
            "breeches", "graffiti", "rabies", "britches", "headquarters", "salmon",
            "carp", "----", "scissors", "ch----is", "high-jinks", "sea-bass", 
            "clippers", "homework", "series", "cod", "innings", "shears", "contretemps", 
            "jackanapes", "species", "corps", "mackerel", "swine", "debris", "measles",
            "trout", "diabetes", "mews", "tuna", "djinn", "mumps", "whiting", "eland", 
            "news", "wildebeest", "elk", "pincers", "police", "hair", "ice", "chaos",
            "milk", "cotton", "pneumonoultramicroscopicsilicovolcanoconiosis",
            "information", "aircraft", "scabies", "traffic", "corn", "millet", "rice",
            "hay", "----", "tobacco", "cabbage", "okra", "broccoli", "asparagus", 
            "lettuce", "beef", "pork", "venison", "mutton",  "cattle", "offspring",
            "molasses", "shambles", "shingles"};

        private readonly Dictionary<string, string> _irregularVerbList =
            new Dictionary<string, string>
            {
                {"am", "are"}, {"are", "are"}, {"is", "are"}, {"was", "were"}, {"were", "were"},
                {"has", "have"}, {"have", "have"}
            };

        private readonly List<string> _pronounList =
            new List<string>
            {
                    "I", "we", "you", "he", "she", "they", "it", 
                    "me", "us", "him", "her", "them",
                    "myself", "ourselves", "yourself", "himself", "herself", "itself",
                    "oneself", "oneselves",
                    "my", "our", "your", "his", "their", "its", 
                    "mine", "yours", "hers", "theirs", "this", "that", "these", "those",
                    "all", "another", "any", "anybody", "anyone", "anything", "both", "each", 
                    "other", "either", "everyone", "everybody", "everything", "most", "much", "nothing", 
                    "nobody", "none", "one", "others", "some", "somebody", "someone", "something",
                    "what", "whatever", "which", "whichever", "who", "whoever", "whom", "whomever", 
                    "whose",
                };

        private readonly Dictionary<string, string> _irregularPluralsDictionary =
            new Dictionary<string, string>
            { 
                    {"brother", "brothers"}, {"child", "children"}, 
                    {"cow", "cows"}, {"ephemeris", "ephemerides"}, {"genie", "genies"},
                    {"money", "moneys"}, {"mongoose", "mongooses"}, {"mythos", "mythoi"}, 
                    {"octopus", "octopuses"}, {"ox", "oxen"}, {"soliloquy", "soliloquies"},
                    {"trilby", "trilbys"}, {"crisis", "crises"}, {"synopsis","synopses"},
                    {"rose", "roses"}, {"gas","gases"}, {"bus", "buses"},
                    {"axis", "axes"},{"memo", "memos"}, {"casino","casinos"}, 
                    {"silo", "silos"},{"stereo", "stereos"}, {"studio","studios"},
                    {"lens", "lenses"}, {"alias","aliases"}, 
                    {"pie","pies"}, {"corpus","corpora"}, 
                    {"viscus", "viscera"},{"hippopotamus", "hippopotami"}, {"trace", "traces"},
                    {"person", "people"}, {"chili", "chilies"}, {"analysis", "analyses"}, 
                    {"basis", "bases"}, {"neurosis", "neuroses"}, {"oasis", "oases"},
                    {"synthesis", "syntheses"}, {"thesis", "theses"}, {"change", "changes"},
                    {"lie", "lies"}, {"calorie", "calories"}, {"freebie", "freebies"}, {"case", "cases"},
                    {"house", "houses"}, {"valve", "valves"}, {"cloth", "clothes"}, {"tie", "ties"}, 
                    {"movie", "movies"}, {"bonus", "bonuses"}, {"specimen", "specimens"}
                };

        readonly Dictionary<string, string> _assimilatedClassicalInflectionDictionary =
                    new Dictionary<string, string>
                    {
                        {"alumna", "alumnae"}, {"alga", "algae"}, {"vertebra", "vertebrae"},
                        {"codex", "codices"},
                        {"murex", "murices"}, {"silex", "silices"}, {"aphelion", "aphelia"}, 
                        {"hyperbaton", "hyperbata"}, {"perihelion", "perihelia"},
                        {"asyndeton", "asyndeta"}, {"noumenon", "noumena"}, 
                        {"phenomenon", "phenomena"}, {"criterion", "criteria"}, {"organon", "organa"}, 
                        {"prolegomenon", "prolegomena"}, {"agendum", "agenda"}, {"datum", "data"},
                        {"extremum", "extrema"}, {"bacterium", "bacteria"}, {"desideratum", "desiderata"}, 
                        {"stratum", "strata"}, {"candelabrum", "candelabra"}, {"erratum", "errata"},
                        {"ovum", "ova"}, {"forum", "fora"}, {"addendum", "addenda"},  {"stadium", "stadia"},
                        {"automaton", "automata"}, {"polyhedron", "polyhedra"},
                    };

        readonly Dictionary<string, string> _oSuffixDictionary =
                    new Dictionary<string, string>
                    {
                        {"albino", "albinos"}, {"generalissimo", "generalissimos"}, 
                        {"manifesto", "manifestos"}, {"archipelago", "archipelagos"},
                        {"ghetto", "ghettos"}, {"medico", "medicos"}, {"armadillo", "armadillos"},
                        {"guano", "guanos"}, {"octavo", "octavos"}, {"commando", "commandos"},
                        {"inferno", "infernos"}, {"photo", "photos"}, {"ditto", "dittos"}, 
                        {"jumbo", "jumbos"}, {"pro", "pros"}, {"dynamo", "dynamos"},
                        {"lingo", "lingos"}, {"quarto", "quartos"}, {"embryo", "embryos"}, 
                        {"lumbago", "lumbagos"}, {"rhino", "rhinos"}, {"fiasco", "fiascos"}, 
                        {"magneto", "magnetos"}, {"stylo", "stylos"}
                    };

        readonly Dictionary<string, string> _classicalInflectionDictionary =
                    new Dictionary<string, string>
                    { 
                        {"stamen", "stamina"}, {"foramen", "foramina"}, {"lumen", "lumina"},
                        {"anathema", "anathemata"}, {"----", "----ta"}, {"oedema", "oedemata"}, 
                        {"bema", "bemata"}, {"enigma", "enigmata"}, {"sarcoma", "sarcomata"}, 
                        {"carcinoma", "carcinomata"}, {"gumma", "gummata"}, {"schema", "schemata"},
                        {"charisma", "charismata"}, {"lemma", "lemmata"}, {"soma", "somata"}, 
                        {"diploma", "diplomata"}, {"lymphoma", "lymphomata"}, {"stigma", "stigmata"},
                        {"dogma", "dogmata"}, {"magma", "magmata"}, {"stoma", "stomata"},
                        {"drama", "dramata"}, {"melisma", "melismata"}, {"trauma", "traumata"},
                        {"edema", "edemata"}, {"miasma", "miasmata"}, {"abscissa", "abscissae"}, 
                        {"formula", "formulae"}, {"medusa", "medusae"}, {"amoeba", "amoebae"},
                        {"hydra", "hydrae"}, {"nebula", "nebulae"}, {"antenna", "antennae"}, 
                        {"hyperbola", "hyperbolae"}, {"nova", "novae"}, {"aurora", "aurorae"}, 
                        {"lacuna", "lacunae"}, {"parabola", "parabolae"}, {"apex", "apices"},
                        {"latex", "latices"}, {"vertex", "vertices"}, {"cortex", "cortices"}, 
                        {"pontifex", "pontifices"}, {"vortex", "vortices"}, {"index", "indices"},
                        {"simplex", "simplices"}, {"iris", "irides"}, {"----oris", "----orides"},
                        {"alto", "alti"}, {"contralto", "contralti"}, {"soprano", "soprani"},
                        {"b----o", "b----i"}, {"crescendo", "crescendi"}, {"tempo", "tempi"}, 
                        {"canto", "canti"}, {"solo", "soli"}, {"aquarium", "aquaria"},
                        {"interregnum", "interregna"}, {"quantum", "quanta"}, 
                        {"compendium", "compendia"}, {"lustrum", "lustra"}, {"rostrum", "rostra"}, 
                        {"consortium", "consortia"}, {"maximum", "maxima"}, {"spectrum", "spectra"},
                        {"cranium", "crania"}, {"medium", "media"}, {"speculum", "specula"}, 
                        {"curriculum", "curricula"}, {"memorandum", "memoranda"}, {"stadium", "stadia"},
                        {"dictum", "dicta"}, {"millenium", "millenia"}, {"t----zium", "t----zia"},
                        {"emporium", "emporia"}, {"minimum", "minima"}, {"ultimatum", "ultimata"},
                        {"enconium", "enconia"}, {"momentum", "momenta"}, {"vacuum", "vacua"}, 
                        {"gymnasium", "gymnasia"}, {"optimum", "optima"}, {"velum", "vela"},
                        {"honorarium", "honoraria"}, {"phylum", "phyla"}, {"focus", "foci"}, 
                        {"nimbus", "nimbi"}, {"succubus", "succubi"}, {"fungus", "fungi"}, 
                        {"nucleolus", "nucleoli"}, {"torus", "tori"}, {"genius", "genii"},
                        {"radius", "radii"}, {"umbilicus", "umbilici"}, {"incubus", "incubi"}, 
                        {"stylus", "styli"}, {"uterus", "uteri"}, {"stimulus", "stimuli"}, {"apparatus", "apparatus"},
                        {"impetus", "impetus"}, {"prospectus", "prospectus"}, {"cantus", "cantus"},
                        {"nexus", "nexus"}, {"sinus", "sinus"}, {"coitus", "coitus"}, {"plexus", "plexus"},
                        {"status", "status"}, {"hiatus", "hiatus"}, {"afreet", "afreeti"}, 
                        {"afrit", "afriti"}, {"efreet", "efreeti"}, {"cherub", "cherubim"},
                        {"goy", "goyim"}, {"seraph", "seraphim"}, {"alumnus", "alumni"} 
                    };

        // this list contains all the plural words that being treated as singluar form, for example, "they" -> "they" 
        private readonly List<string> _knownConflictingPluralList =
            new List<string>
            {
                "they", "them", "their", "have", "were", "yourself", "are" 
            };

        // this list contains the words ending with "se" and we special case these words since 
        // we need to add a rule for "ses" singularize to "s"
        private readonly Dictionary<string, string> _wordsEndingWithSeDictionary =
            new Dictionary<string, string>
            {
                {"house", "houses"}, {"case", "cases"}, {"enterprise", "enterprises"},
                {"purchase", "purchases"}, {"surprise", "surprises"}, {"release", "releases"}, 
                {"disease", "diseases"}, {"promise", "promises"}, {"refuse", "refuses"},
                {"whose", "whoses"}, {"phase", "phases"}, {"noise", "noises"}, 
                {"nurse", "nurses"}, {"rose", "roses"}, {"franchise", "franchises"}, 
                {"supervise", "supervises"}, {"farmhouse", "farmhouses"},
                {"suitcase", "suitcases"}, {"recourse", "recourses"}, {"impulse", "impulses"}, 
                {"license", "licenses"}, {"diocese", "dioceses"}, {"excise", "excises"},
                {"demise", "demises"}, {"blouse", "blouses"},
                {"bruise", "bruises"}, {"misuse", "misuses"}, {"curse", "curses"},
                {"prose", "proses"}, {"purse", "purses"}, {"goose", "gooses"}, 
                {"tease", "teases"}, {"poise", "poises"}, {"vase", "vases"},
                {"fuse", "fuses"}, {"muse", "muses"}, 
                {"slaughterhouse", "slaughterhouses"}, {"clearinghouse", "clearinghouses"}, 
                {"endonuclease", "endonucleases"}, {"steeplechase", "steeplechases"},
                {"metamorphose", "metamorphoses"}, {"----", "----s"}, 
                {"commonsense", "commonsenses"}, {"intersperse", "intersperses"},
                {"merchandise", "merchandises"}, {"phosphatase", "phosphatases"},
                {"summerhouse", "summerhouses"}, {"watercourse", "watercourses"},
                {"catchphrase", "catchphrases"}, {"compromise", "compromises"}, 
                {"greenhouse", "greenhouses"}, {"lighthouse", "lighthouses"},
                {"paraphrase", "paraphrases"}, {"mayonnaise", "mayonnaises"}, 
                {"----course", "----courses"}, {"apocalypse", "apocalypses"}, 
                {"courthouse", "courthouses"}, {"powerhouse", "powerhouses"},
                {"storehouse", "storehouses"}, {"glasshouse", "glasshouses"}, 
                {"hypotenuse", "hypotenuses"}, {"peroxidase", "peroxidases"},
                {"pillowcase", "pillowcases"}, {"roundhouse", "roundhouses"},
                {"streetwise", "streetwises"}, {"expertise", "expertises"},
                {"discourse", "discourses"}, {"warehouse", "warehouses"}, 
                {"staircase", "staircases"}, {"workhouse", "workhouses"},
                {"briefcase", "briefcases"}, {"clubhouse", "clubhouses"}, 
                {"clockwise", "clockwises"}, {"concourse", "concourses"}, 
                {"playhouse", "playhouses"}, {"turquoise", "turquoises"},
                {"boathouse", "boathouses"}, {"cellulose", "celluloses"}, 
                {"epitomise", "epitomises"}, {"gatehouse", "gatehouses"},
                {"grandiose", "grandioses"}, {"menopause", "menopauses"},
                {"penthouse", "penthouses"}, {"----horse", "----horses"},
                {"transpose", "transposes"}, {"almshouse", "almshouses"}, 
                {"customise", "customises"}, {"footloose", "footlooses"},
                {"galvanise", "galvanises"}, {"princesse", "princesses"}, 
                {"universe", "universes"}, {"workhorse", "workhorses"} 
            };

        private readonly Dictionary<string, string> _wordsEndingWithSisDictionary =
            new Dictionary<string, string>
            {
                {"analysis", "analyses"}, {"crisis", "crises"}, {"basis", "bases"}, 
                {"atherosclerosis", "atheroscleroses"}, {"electrophoresis", "electrophoreses"},
                {"psychoanalysis", "psychoanalyses"}, {"photosynthesis", "photosyntheses"}, 
                {"amniocentesis", "amniocenteses"}, {"metamorphosis", "metamorphoses"}, 
                {"toxoplasmosis", "toxoplasmoses"}, {"endometriosis", "endometrioses"},
                {"tuberculosis", "tuberculoses"}, {"pathogenesis", "pathogeneses"}, 
                {"osteoporosis", "osteoporoses"}, {"parenthesis", "parentheses"},
                {"anastomosis", "anastomoses"}, {"peristalsis", "peristalses"},
                {"hypothesis", "hypotheses"}, {"antithesis", "antitheses"},
                {"apotheosis", "apotheoses"}, {"thrombosis", "thromboses"}, 
                {"diagnosis", "diagnoses"}, {"synthesis", "syntheses"},
                {"paralysis", "paralyses"}, {"prognosis", "prognoses"}, 
                {"cirrhosis", "cirrhoses"}, {"sclerosis", "scleroses"}, 
                {"psychosis", "psychoses"}, {"apoptosis", "apoptoses"}, {"symbiosis", "symbioses"}
            };

        private readonly Dictionary<string, string> _wordsEndingWithSusDictionary =
            new Dictionary<string, string>
            { 
                {"consensus","consensuses"}, {"census", "censuses"}
            };

        private readonly Dictionary<string, string> _wordsEndingWithInxAnxYnxDictionary =
            new Dictionary<string, string>
            {
                {"sphinx", "sphinxes"},
                {"larynx", "larynges"}, {"lynx", "lynxes"}, {"pharynx", "pharynxes"},
                {"phalanx", "phalanxes"} 
            };

        /// <summary>
        /// 
        /// </summary>
        private EnglishPluralizationService()
        {
            Culture = new CultureInfo("en");

            _userDictionary = new BidirectionalDictionary<string, string>();

            _irregularPluralsPluralizationService =
                new StringBidirectionalDictionary(_irregularPluralsDictionary);
            _assimilatedClassicalInflectionPluralizationService =
                new StringBidirectionalDictionary(_assimilatedClassicalInflectionDictionary);
            _oSuffixPluralizationService =
                new StringBidirectionalDictionary(_oSuffixDictionary);
            _classicalInflectionPluralizationService =
                new StringBidirectionalDictionary(_classicalInflectionDictionary);
            _wordsEndingWithSePluralizationService =
                new StringBidirectionalDictionary(_wordsEndingWithSeDictionary);
            _wordsEndingWithSisPluralizationService =
                new StringBidirectionalDictionary(_wordsEndingWithSisDictionary);
            _wordsEndingWithSusPluralizationService =
                new StringBidirectionalDictionary(_wordsEndingWithSusDictionary);
            _wordsEndingWithInxAnxYnxPluralizationService =
                new StringBidirectionalDictionary(_wordsEndingWithInxAnxYnxDictionary);

            // verb
            _irregularVerbPluralizationService =
                new StringBidirectionalDictionary(_irregularVerbList);

            _knownSingluarWords = new List<string>(
                _irregularPluralsDictionary.Keys
                .Concat(_assimilatedClassicalInflectionDictionary.Keys)
                .Concat(_oSuffixDictionary.Keys)
                .Concat(_classicalInflectionDictionary.Keys)
                .Concat(_irregularVerbList.Keys)
                .Concat(_irregularPluralsDictionary.Keys)
                .Concat(_wordsEndingWithSeDictionary.Keys)
                .Concat(_wordsEndingWithSisDictionary.Keys)
                .Concat(_wordsEndingWithSusDictionary.Keys)
                .Concat(_wordsEndingWithInxAnxYnxDictionary.Keys)
                .Concat(_uninflectiveWordList)
                .Except(_knownConflictingPluralList)); // see the _knowConflictingPluralList comment above

            _knownPluralWords = new List<string>(
                _irregularPluralsDictionary.Values
                .Concat(_assimilatedClassicalInflectionDictionary.Values)
                .Concat(_oSuffixDictionary.Values)
                .Concat(_classicalInflectionDictionary.Values)
                .Concat(_irregularVerbList.Values)
                .Concat(_irregularPluralsDictionary.Values)
                .Concat(_wordsEndingWithSeDictionary.Values)
                .Concat(_wordsEndingWithSisDictionary.Values)
                .Concat(_wordsEndingWithSusDictionary.Values)
                .Concat(_wordsEndingWithInxAnxYnxDictionary.Values)
                .Concat(_uninflectiveWordList));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        public bool IsPlural(string word)
        {
            CheckArgumentNull(word, "word");

            if (_userDictionary.ExistsInSecond(word))
            {
                return true;
            }
            if (_userDictionary.ExistsInFirst(word))
            {
                return false;
            }

            if (IsUninflective(word) || _knownPluralWords.Contains(word.ToLower(Culture)))
            {
                return true;
            }
            if (Singularize(word).Equals(word))
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        public bool IsSingular(string word)
        {
            CheckArgumentNull(word, "word");

            if (_userDictionary.ExistsInFirst(word))
            {
                return true;
            }
            if (_userDictionary.ExistsInSecond(word))
            {
                return false;
            }

            if (IsUninflective(word) || _knownSingluarWords.Contains(word.ToLower(Culture)))
            {
                return true;
            }
            if (!IsNoOpWord(word) && Singularize(word).Equals(word))
            {
                return true;
            }
            return false;
        }

        //
        /// <summary>
        /// 
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        public string Pluralize(string word)
        {
            CheckArgumentNull(word, "word");

            return Capitalize(word, PublicPluralize);
        }

        [SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase")]
        private string PublicPluralize(string word)
        {
            // words that we know of
            if (_userDictionary.ExistsInFirst(word))
            {
                return _userDictionary.GetSecondValue(word);
            }

            if (IsNoOpWord(word))
            {
                return word;
            }

            string prefixWord;
            string suffixWord = GetSuffixWord(word, out prefixWord);

            // by me -> by me 
            if (IsNoOpWord(suffixWord))
            {
                return prefixWord + suffixWord;
            }

            // handle the word that do not inflect in the plural form
            if (IsUninflective(suffixWord))
            {
                return prefixWord + suffixWord;
            }

            // if word is one of the known plural forms, then just return 
            if (_knownPluralWords.Contains(suffixWord.ToLowerInvariant()) || IsPlural(suffixWord))
            {
                return prefixWord + suffixWord;
            }

            // handle irregular plurals, e.g. "ox" -> "oxen" 
            if (_irregularPluralsPluralizationService.ExistsInFirst(suffixWord))
            {
                return prefixWord + _irregularPluralsPluralizationService.GetSecondValue(suffixWord);
            }

            string newSuffixWord;
            if (TryInflectOnSuffixInWord(suffixWord,
                new List<string> { "man" },
                s => s.Remove(s.Length - 2, 2) + "en", Culture, out newSuffixWord))
            {
                return prefixWord + newSuffixWord;
            }

            // handle irregular inflections for common suffixes, e.g. "mouse" -> "mice"
            if (TryInflectOnSuffixInWord(suffixWord,
                new List<string> { "louse", "mouse" },
                s => s.Remove(s.Length - 4, 4) + "ice", Culture, out newSuffixWord))
            {
                return prefixWord + newSuffixWord;
            }

            if (TryInflectOnSuffixInWord(suffixWord,
                new List<string> { "tooth" },
                s => s.Remove(s.Length - 4, 4) + "eeth", Culture, out newSuffixWord))
            {
                return prefixWord + newSuffixWord;
            }
            if (TryInflectOnSuffixInWord(suffixWord,
                new List<string> { "goose" },
                s => s.Remove(s.Length - 4, 4) + "eese", Culture, out newSuffixWord))
            {
                return prefixWord + newSuffixWord;
            }
            if (TryInflectOnSuffixInWord(suffixWord,
                new List<string> { "foot" },
                s => s.Remove(s.Length - 3, 3) + "eet", Culture, out newSuffixWord))
            {
                return prefixWord + newSuffixWord;
            }
            if (TryInflectOnSuffixInWord(suffixWord,
                new List<string> { "zoon" },
                s => s.Remove(s.Length - 3, 3) + "oa", Culture, out newSuffixWord))
            {
                return prefixWord + newSuffixWord;
            }
            if (TryInflectOnSuffixInWord(suffixWord,
                new List<string> { "cis", "sis", "xis" },
                s => s.Remove(s.Length - 2, 2) + "es", Culture, out newSuffixWord))
            {
                return prefixWord + newSuffixWord;
            }

            // handle assimilated classical inflections, e.g. vertebra -> vertebrae
            if (_assimilatedClassicalInflectionPluralizationService.ExistsInFirst(suffixWord))
            {
                return prefixWord + _assimilatedClassicalInflectionPluralizationService.GetSecondValue(suffixWord);
            }

            // Handle the classical variants of modern inflections
            //
            if (_classicalInflectionPluralizationService.ExistsInFirst(suffixWord))
            {
                return prefixWord + _classicalInflectionPluralizationService.GetSecondValue(suffixWord);
            }

            if (TryInflectOnSuffixInWord(suffixWord,
                new List<string> { "trix" },
                s => s.Remove(s.Length - 1, 1) + "ces", Culture, out newSuffixWord))
            {
                return prefixWord + newSuffixWord;
            }

            if (TryInflectOnSuffixInWord(suffixWord,
                new List<string> { "eau", "ieu" },
                s => s + "x", Culture, out newSuffixWord))
            {
                return prefixWord + newSuffixWord;
            }

            if (_wordsEndingWithInxAnxYnxPluralizationService.ExistsInFirst(suffixWord))
            {
                return prefixWord + _wordsEndingWithInxAnxYnxPluralizationService.GetSecondValue(suffixWord);
            }

            // [cs]h and ss that take es as plural form
            if (TryInflectOnSuffixInWord(suffixWord, new List<string> { "ch", "sh", "ss" }, s => s + "es", Culture, out newSuffixWord))
            {
                return prefixWord + newSuffixWord;
            }

            // f, fe that take ves as plural form 
            if (TryInflectOnSuffixInWord(suffixWord,
                new List<string> { "alf", "elf", "olf", "eaf", "arf" },
                s => s.EndsWith("deaf", true, Culture) ? s : s.Remove(s.Length - 1, 1) + "ves", Culture, out newSuffixWord))
            {
                return prefixWord + newSuffixWord;
            }

            if (TryInflectOnSuffixInWord(suffixWord,
                new List<string> { "nife", "life", "wife" },
                s => s.Remove(s.Length - 2, 2) + "ves", Culture, out newSuffixWord))
            {
                return prefixWord + newSuffixWord;
            }

            // y takes ys as plural form if preceded by a vowel, but ies if preceded by a consonant, e.g. stays, skies 
            if (TryInflectOnSuffixInWord(suffixWord,
                new List<string> { "ay", "ey", "iy", "oy", "uy" },
                s => s + "s", Culture, out newSuffixWord))
            {
                return prefixWord + newSuffixWord;
            }

            //

            if (suffixWord.EndsWith("y", true, Culture))
            {
                return prefixWord + suffixWord.Remove(suffixWord.Length - 1, 1) + "ies";
            }

            // handle some of the words o -> os, and [vowel]o -> os, and the rest are o->oes
            if (_oSuffixPluralizationService.ExistsInFirst(suffixWord))
            {
                return prefixWord + _oSuffixPluralizationService.GetSecondValue(suffixWord);
            }

            if (TryInflectOnSuffixInWord(suffixWord,
                new List<string> { "ao", "eo", "io", "oo", "uo" },
                s => s + "s", Culture, out newSuffixWord))
            {
                return prefixWord + newSuffixWord;
            }

            if (suffixWord.EndsWith("o", true, Culture) || suffixWord.EndsWith("s", true, Culture))
            {
                return prefixWord + suffixWord + "es";
            }

            if (suffixWord.EndsWith("x", true, Culture))
            {
                return prefixWord + suffixWord + "es";
            }

            // cats, bags, hats, speakers 
            return prefixWord + suffixWord + "s";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        public string Singularize(string word)
        {
            CheckArgumentNull(word, "word");

            return Capitalize(word, PublicSingularize);
        }

        [SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase")]
        private string PublicSingularize(string word)
        {
            // words that we know of
            if (_userDictionary.ExistsInSecond(word))
            {
                return _userDictionary.GetFirstValue(word);
            }

            if (IsNoOpWord(word))
            {
                return word;
            }

            string prefixWord;
            string suffixWord = GetSuffixWord(word, out prefixWord);

            if (IsNoOpWord(suffixWord))
            {
                return prefixWord + suffixWord;
            }

            // handle the word that is the same as the plural form
            if (IsUninflective(suffixWord))
            {
                return prefixWord + suffixWord;
            }

            // if word is one of the known singular words, then just return 

            if (_knownSingluarWords.Contains(suffixWord.ToLowerInvariant()))
            {
                return prefixWord + suffixWord;
            }

            // handle simple irregular verbs, e.g. was -> were 
            if (_irregularVerbPluralizationService.ExistsInSecond(suffixWord))
            {
                return prefixWord + _irregularVerbPluralizationService.GetFirstValue(suffixWord);
            }

            // handle irregular plurals, e.g. "ox" -> "oxen" 
            if (_irregularPluralsPluralizationService.ExistsInSecond(suffixWord))
            {
                return prefixWord + _irregularPluralsPluralizationService.GetFirstValue(suffixWord);
            }

            // handle singluarization for words ending with sis and pluralized to ses,
            // e.g. "ses" -> "sis"
            if (_wordsEndingWithSisPluralizationService.ExistsInSecond(suffixWord))
            {
                return prefixWord + _wordsEndingWithSisPluralizationService.GetFirstValue(suffixWord);
            }

            // handle words ending with se, e.g. "ses" -> "se"
            if (_wordsEndingWithSePluralizationService.ExistsInSecond(suffixWord))
            {
                return prefixWord + _wordsEndingWithSePluralizationService.GetFirstValue(suffixWord);
            }

            // handle words ending with sus, e.g. "suses" -> "sus"
            if (_wordsEndingWithSusPluralizationService.ExistsInSecond(suffixWord))
            {
                return prefixWord + _wordsEndingWithSusPluralizationService.GetFirstValue(suffixWord);
            }

            string newSuffixWord;
            if (TryInflectOnSuffixInWord(suffixWord,
                new List<string> { "men" },
                s => s.Remove(s.Length - 2, 2) + "an", Culture, out newSuffixWord))
            {
                return prefixWord + newSuffixWord;
            }

            // handle irregular inflections for common suffixes, e.g. "mouse" -> "mice"
            if (TryInflectOnSuffixInWord(suffixWord,
                new List<string> { "lice", "mice" },
                s => s.Remove(s.Length - 3, 3) + "ouse", Culture, out newSuffixWord))
            {
                return prefixWord + newSuffixWord;
            }

            if (TryInflectOnSuffixInWord(suffixWord,
                new List<string> { "teeth" },
                s => s.Remove(s.Length - 4, 4) + "ooth", Culture, out newSuffixWord))
            {
                return prefixWord + newSuffixWord;
            }
            if (TryInflectOnSuffixInWord(suffixWord,
                new List<string> { "geese" },
                s => s.Remove(s.Length - 4, 4) + "oose", Culture, out newSuffixWord))
            {
                return prefixWord + newSuffixWord;
            }
            if (TryInflectOnSuffixInWord(suffixWord,
                new List<string> { "feet" },
                s => s.Remove(s.Length - 3, 3) + "oot", Culture, out newSuffixWord))
            {
                return prefixWord + newSuffixWord;
            }
            if (TryInflectOnSuffixInWord(suffixWord,
                new List<string> { "zoa" },
                s => s.Remove(s.Length - 2, 2) + "oon", Culture, out newSuffixWord))
            {
                return prefixWord + newSuffixWord;
            }

            // [cs]h and ss that take es as plural form, this is being moved up since the sses will be override by the ses 
            if (TryInflectOnSuffixInWord(suffixWord,
                new List<string> { "ches", "shes", "sses" },
                s => s.Remove(s.Length - 2, 2), Culture, out newSuffixWord))
            {
                return prefixWord + newSuffixWord;
            }


            // handle assimilated classical inflections, e.g. vertebra -> vertebrae 
            if (_assimilatedClassicalInflectionPluralizationService.ExistsInSecond(suffixWord))
            {
                return prefixWord + _assimilatedClassicalInflectionPluralizationService.GetFirstValue(suffixWord);
            }

            // Handle the classical variants of modern inflections 
            //
            if (_classicalInflectionPluralizationService.ExistsInSecond(suffixWord))
            {
                return prefixWord + _classicalInflectionPluralizationService.GetFirstValue(suffixWord);
            }

            if (TryInflectOnSuffixInWord(suffixWord,
                new List<string> { "trices" },
                s => s.Remove(s.Length - 3, 3) + "x", Culture, out newSuffixWord))
            {
                return prefixWord + newSuffixWord;
            }

            if (TryInflectOnSuffixInWord(suffixWord,
                new List<string> { "eaux", "ieux" },
                s => s.Remove(s.Length - 1, 1), Culture, out newSuffixWord))
            {
                return prefixWord + newSuffixWord;
            }

            if (_wordsEndingWithInxAnxYnxPluralizationService.ExistsInSecond(suffixWord))
            {
                return prefixWord + _wordsEndingWithInxAnxYnxPluralizationService.GetFirstValue(suffixWord);
            }

            // f, fe that take ves as plural form
            if (TryInflectOnSuffixInWord(suffixWord,
                new List<string> { "alves", "elves", "olves", "eaves", "arves" },
                s => s.Remove(s.Length - 3, 3) + "f", Culture, out newSuffixWord))
            {
                return prefixWord + newSuffixWord;
            }

            if (TryInflectOnSuffixInWord(suffixWord,
                new List<string> { "nives", "lives", "wives" },
                s => s.Remove(s.Length - 3, 3) + "fe", Culture, out newSuffixWord))
            {
                return prefixWord + newSuffixWord;
            }

            // y takes ys as plural form if preceded by a vowel, but ies if preceded by a consonant, e.g. stays, skies 
            if (TryInflectOnSuffixInWord(suffixWord,
                new List<string> { "ays", "eys", "iys", "oys", "uys" },
                s => s.Remove(s.Length - 1, 1), Culture, out newSuffixWord))
            {
                return prefixWord + newSuffixWord;
            }

            //

            if (suffixWord.EndsWith("ies", true, Culture))
            {
                return prefixWord + suffixWord.Remove(suffixWord.Length - 3, 3) + "y";
            }

            // handle some of the words o -> os, and [vowel]o -> os, and the rest are o->oes 
            if (_oSuffixPluralizationService.ExistsInSecond(suffixWord))
            {
                return prefixWord + _oSuffixPluralizationService.GetFirstValue(suffixWord);
            }

            if (TryInflectOnSuffixInWord(suffixWord,
                new List<string> { "aos", "eos", "ios", "oos", "uos" },
                s => suffixWord.Remove(suffixWord.Length - 1, 1), Culture, out newSuffixWord))
            {
                return prefixWord + newSuffixWord;
            }

            //




            if (TryInflectOnSuffixInWord(suffixWord,
                new List<string> { "ces" },
                s => s.Remove(s.Length - 1, 1), Culture, out newSuffixWord))
            {
                return prefixWord + newSuffixWord;
            }

            if (TryInflectOnSuffixInWord(suffixWord,
                new List<string> { "ces", "ses", "xes" },
                s => s.Remove(s.Length - 2, 2), Culture, out newSuffixWord))
            {
                return prefixWord + newSuffixWord;
            }

            if (suffixWord.EndsWith("oes", true, Culture))
            {
                return prefixWord + suffixWord.Remove(suffixWord.Length - 2, 2);
            }

            if (suffixWord.EndsWith("ss", true, Culture))
            {
                return prefixWord + suffixWord;
            }

            if (suffixWord.EndsWith("s", true, Culture))
            {
                return prefixWord + suffixWord.Remove(suffixWord.Length - 1, 1);
            }

            // word is a singlar
            return prefixWord + suffixWord;
        }

        #region Utils
        /// <summary> 
        /// captalize the return word if the parameter is capitalized
        /// if word is "Table", then return "Tables" 
        /// </summary> 
        /// <param name="word"></param>
        /// <param name="action"></param> 
        /// <returns></returns>
        private string Capitalize(string word, Func<string, string> action)
        {
            string result = action(word);

            if (IsCapitalized(word))
            {
                if (result.Length == 0)
                    return result;

                var sb = new StringBuilder(result.Length);

                sb.Append(char.ToUpperInvariant(result[0]));
                sb.Append(result.Substring(1));
                return sb.ToString();
            }
            return result;
        }

        /// <summary>
        /// separate one combine word in to two parts, prefix word and the last word(suffix word) 
        /// </summary> 
        /// <param name="word"></param>
        /// <param name="prefixWord"></param> 
        /// <returns></returns>
        private string GetSuffixWord(string word, out string prefixWord)
        {
            // use the last space to separate the words 
            int lastSpaceIndex = word.LastIndexOf(' ');
            prefixWord = word.Substring(0, lastSpaceIndex + 1);
            return word.Substring(lastSpaceIndex + 1);

            // 
        }

        private bool IsCapitalized(string word)
        {
            return !string.IsNullOrEmpty(word) && char.IsUpper(word, 0);
        }

        private bool IsAlphabets(string word)
        {
            // return false when the word is "[\s]*" or leading or tailing with spaces
            // or contains non alphabetical characters
            return !string.IsNullOrEmpty(word.Trim()) && word.Equals(word.Trim()) && !Regex.IsMatch(word, "[^a-zA-Z\\s]");
        }

        [SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase")]
        private bool IsUninflective(string word)
        {
            CheckArgumentNull(word, "word");
            return DoesWordContainSuffix(word, _uninflectiveSuffixList, Culture)
                   || (!word.ToLower(Culture).Equals(word) && word.EndsWith("ese", false, Culture))
                   || _uninflectiveWordList.Contains(word.ToLowerInvariant());
        }

        /// <summary>
        /// return true when the word is "[\s]*" or leading or tailing with spaces 
        /// or contains non alphabetical characters
        /// </summary> 
        /// <param name="word"></param> 
        /// <returns></returns>
        [SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase")]
        private bool IsNoOpWord(string word)
        {
            return !IsAlphabets(word) ||
                   word.Length <= 1 ||
                   _pronounList.Contains(word.ToLowerInvariant());
        }

        #endregion

        #region ICustomPluralizationMapping Members

        /// <summary>
        /// This method allow you to add word to public PluralizationService of English.
        /// If the singluar or the plural value was already added by this method, then an ArgumentException will be thrown. 
        /// </summary>
        /// <param name="singular"></param> 
        /// <param name="plural"></param> 
        public void AddWord(string singular, string plural)
        {
            CheckArgumentNull(singular, "singular");
            CheckArgumentNull(plural, "plural");

            if (_userDictionary.ExistsInSecond(plural))
            {
                throw new ArgumentException("plural");
            }
            if (_userDictionary.ExistsInFirst(singular))
            {
                throw new ArgumentException("singular");
            }
            _userDictionary.AddValue(singular, plural);
        }

        #endregion
    }
}

// File provided for Reference Use Only by Microsoft Corporation (c) 2007.
