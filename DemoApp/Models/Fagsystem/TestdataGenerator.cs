using DemoApp.Pages.Partial;

namespace DemoApp.Models.Fagsystem
{
    public static class TestdataGenerator
    {

        private static readonly List<Barn> testbarn = GenerateTestdata();

        public static List<Barn> GetTestdata()
        {
            return testbarn;
        }

        private static List<FREG_Person> addFregData()
        {
            List<FREG_Person> personer =
            [
                new()
                {
                    Id = 10000001,
                    Fornavn = "Bjarne",
                    Etternavn = "Wern",
                    Fodselsdato = new DateTime(2012, 12, 12),
                    Fodselsnummer = "12121241555",
                    Graderingsnivaa = AdresseGradering.ugradert,
                    Kjonn = "1"
                },
                new()
                {
                    Id = 10000002,
                    Fornavn = "Birgitte",
                    Etternavn = "Wern",
                    Fodselsdato = new DateTime(2010, 10, 10),
                    Fodselsnummer = "10101041444",
                    Graderingsnivaa = AdresseGradering.ugradert,
                    Kjonn = "2"
                },
                new()
                {
                    Id = 10000003,
                    Fornavn = "Rigmor",
                    Etternavn = "Wern",
                    Fodselsdato = new DateTime(1990, 10, 10),
                    Fodselsnummer = "10109051455",
                    Graderingsnivaa = AdresseGradering.ugradert,
                    Kjonn = "2"
                },
                new()
                {
                    Id = 10000004,
                    Fornavn = "Rigfar",
                    Etternavn = "Hermann",
                    Fodselsdato = new DateTime(1990, 12, 12),
                    Fodselsnummer = "12129051555",
                    Graderingsnivaa = AdresseGradering.ugradert,
                    Kjonn = "1"
                },
                new()
                {
                    Id = 10000005,
                    Fornavn = "Alexander",
                    Etternavn = "Hermann",
                    Fodselsdato = new DateTime(1949, 02, 02),
                    Fodselsnummer = "02024953555",
                    Graderingsnivaa = AdresseGradering.ugradert,
                    Kjonn = "1"
                }
                ];
            return personer;
        }

        private static List<Barn> addBarn()
        {
            List<Barn> barn =
            [
                new Barn()
                {
                    Id = 100010001,
                    _FREG_Person_Id = 10000001,
                    Type = BarnType.Standard,
                    Nettverk = [
                        new NettverkPerson()
                        {
                            Id = 12000001,
                            Barn_Id = 100010001,
                            NettverkRelasjon = NettverkRelasjonType.Mor,
                            FREG_Person_id = 10000003
                        },
                        new NettverkPerson()
                        {
                            Id = 12000002,
                            Barn_Id = 100010001,
                            NettverkRelasjon = NettverkRelasjonType.Far,
                            FREG_Person_id = 10000004
                        },
                        new NettverkPerson()
                        {
                            Id = 12000003,
                            Barn_Id = 100010001,
                            NettverkRelasjon = NettverkRelasjonType.Annet,
                            FREG_Person_id = 10000002
                        },
                        new NettverkPerson()
                        {
                            Id = 12000004,
                            Barn_Id = 100010001,
                            NettverkRelasjon = NettverkRelasjonType.Annet,
                            FREG_Person_id = 10000005
                        }
                    ]
                },
                new Barn()
                {
                    Id = 100010002,
                    _FREG_Person_Id = 10000003,
                    Type = BarnType.Ufodt,
                    Nettverk = [
                        new NettverkPerson()
                        {
                            Id = 12000005,
                            Barn_Id = 100010002,
                            NettverkRelasjon = NettverkRelasjonType.Mor,
                            FREG_Person_id = 10000003
                        },
                        new NettverkPerson()
                        {
                            Id = 12000002,
                            Barn_Id = 100010002,
                            NettverkRelasjon = NettverkRelasjonType.Far,
                            FREG_Person_id = 10000004
                        }
                    ]
                }
            ];
            return barn;
        }

        private static List<Sak> addSaksdata()
        {
            List<Sak> saker =
                [
                    new Sak() // Bjarne Wern
                    {
                        Id = 10000001,
                        BarnId = 10000001,
                        Saksdata = new Dictionary<string, string>()
                        {
                            { "FellesInfo/BarnMedNettverk/BarnetsSituasjon/Omrade", "Omrade=1|Beskrivelse=" },
                        }
                    },

                    new Sak() // Birgitte Wern
                    {
                        Id = 10000002,
                        BarnId = 10000002,
                    }
                    ];
            return saker;
        }

        private static void initBarn(List<Barn> barn, List<FREG_Person> freg)
        {
            foreach (Barn b in barn)
            {
                b.FREG_Person = freg.FirstOrDefault(f => f.Id == b._FREG_Person_Id);
                foreach (NettverkPerson np in b.Nettverk ?? [])
                {
                    np.fREG_Person = freg.FirstOrDefault(f => f.Id == np.FREG_Person_id);
                }
            }
        }

        public static List<Barn> GenerateTestdata()
        {
            var freg = addFregData();
            var barn = addBarn();
            initBarn(barn, freg);
            return barn;
        }

        public static Barneverntjeneste GetBarneverntjeneste()
        {
            return new Barneverntjeneste()
            {
                Navn = "Barneverntjenesten i Asker",
                Organisasjonsnummer = "974 635 453",
                Kommunenummer = "3203",
                KommuneNavn = "Asker",
                Bydelsnavn = "",
                Bydelsnummer = ""
            };
        }

        public static List<PrefilledValue> GetPrefillValues(HttpRequest request)
        {
            List<PrefilledValue> result = [];
            var bvTjeneste = GetBarneverntjeneste();
            result.Add(new("MeldingHeader/Id", Guid.NewGuid().ToString(), false));
            result.Add(new("MeldingHeader/Type", "1", false));
            result.Add(new("MeldingHeader/Avsender/Organisasjonsnummer", bvTjeneste.Organisasjonsnummer, false));
            result.Add(new("MeldingHeader/Avsender/Kommunenummer", bvTjeneste.Kommunenummer, false));
            result.Add(new("MeldingHeader/Avsender/Kommunenavn", bvTjeneste.KommuneNavn, false));
            result.Add(new("MeldingHeader/Avsender/Bydelsnummer", bvTjeneste.Bydelsnummer, false));
            result.Add(new("MeldingHeader/Avsender/Bydelsnavn", bvTjeneste.Bydelsnavn, false));
            result.Add(new("MeldingHeader/Fagsystem/Leverandor", "Visma", false));
            result.Add(new("MeldingHeader/Fagsystem/Navn", "Visma Flyt Barnevern", false));
            result.Add(new("MeldingHeader/Fagsystem/Versjon", "2.2.234", false));

            BarnSelectorModel model = new(request);
            var barn = model.selectedBarn;

            result.Add(new("FellesInfo/Kontaktperson/Navn", "Bjørge Sæther", true));
            result.Add(new("FellesInfo/Kontaktperson/Telefon", "90822239", true));
            result.Add(new("FellesInfo/Kontaktperson/epost", "bjorge.saether@bufdir.no", true));
            result.Add(new("FellesInfo/KontaktpersonLeder/Navn", "Kenneth Normann Hansen", true));
            result.Add(new("FellesInfo/KontaktpersonLeder/Telefon", "90909090", true));
            result.Add(new("FellesInfo/KontaktpersonLeder/epost", "kenneth.hansen@bufdir.no", true));
            result.Add(new("FellesInfo/BarnMedNettverk/BarnInformasjon/Barn/Fodselsnummer", barn.FREG_Person.Fodselsnummer, false));
            result.Add(new("FellesInfo/BarnMedNettverk/BarnInformasjon/Barn/Fodseldato", barn.FREG_Person.Fodselsdato.ToString("yyyy-MM-dd"), false));
            result.Add(new("FellesInfo/BarnMedNettverk/BarnInformasjon/Barn/KjonnFreg", barn.FREG_Person.Kjonn, false));
            result.Add(new("FellesInfo/BarnMedNettverk/BarnInformasjon/Barn/EMA", false.ToString(), false));
            result.Add(new("FellesInfo/BarnMedNettverk/BarnInformasjon/Barn/Ufodt", false.ToString(), false));
            result.Add(new("FellesInfo/BarnMedNettverk/BarnInformasjon/Barn/DUFnummer", "", false));
            result.Add(new("FellesInfo/BarnMedNettverk/BarnInformasjon/TiltakHistorikk", @"01.02.2022 - 12.09.2022 Fosterhjem
12.09.2022 - 20.10.2023 Omsorgsinstitusjon", true));
            return result;
        }

        public static PrefilledValue? GetPrefilledValueFor(List<PrefilledValue> values, string id)
        {
            var hit = values.FirstOrDefault(p => id.EndsWith(p.Xpath));
            return hit;
        }
    }
}
