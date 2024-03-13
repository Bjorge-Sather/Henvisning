

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
                    Kjonn = "mann"
                },
                new()
                {
                    Id = 10000002,
                    Fornavn = "Birgitte",
                    Etternavn = "Wern",
                    Fodselsdato = new DateTime(2010, 10, 10),
                    Fodselsnummer = "10101041444",
                    Graderingsnivaa = AdresseGradering.ugradert,
                    Kjonn = "kvinne"
                },
                new()
                {
                    Id = 10000003,
                    Fornavn = "Rigmor",
                    Etternavn = "Wern",
                    Fodselsdato = new DateTime(1990, 10, 10),
                    Fodselsnummer = "10109051455",
                    Graderingsnivaa = AdresseGradering.ugradert,
                    Kjonn = "kvinne"
                },
                new()
                {
                    Id = 10000004,
                    Fornavn = "Rigfar",
                    Etternavn = "Hermann",
                    Fodselsdato = new DateTime(1990, 12, 12),
                    Fodselsnummer = "12129051555",
                    Graderingsnivaa = AdresseGradering.ugradert,
                    Kjonn = "mann"
                },
                new()
                {
                    Id = 10000005,
                    Fornavn = "Alexander",
                    Etternavn = "Hermann",
                    Fodselsdato = new DateTime(1949, 02, 02),
                    Fodselsnummer = "02024953555",
                    Graderingsnivaa = AdresseGradering.ugradert,
                    Kjonn = "mann"
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
    }
}
