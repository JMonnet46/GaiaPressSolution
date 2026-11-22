using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GaiaPressBlazorWebAPP.Migrations.GaiaPressDb
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LesChantiersDb",
                columns: table => new
                {
                    RefConstructionSite = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    NameConstructionSite = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DescriptifConstructionSite = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateChantier = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RefClient = table.Column<int>(type: "int", nullable: true),
                    Entreprise = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Adresse = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Telephone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Reference = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsSaved = table.Column<bool>(type: "bit", nullable: false),
                    FilePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FileTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EventsSupendus = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LesChantiersDb", x => x.RefConstructionSite);
                });

            migrationBuilder.CreateTable(
                name: "Forages",
                columns: table => new
                {
                    RefForage = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ConstructionSiteRefConstructionSite = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Forages", x => x.RefForage);
                    table.ForeignKey(
                        name: "FK_Forages_LesChantiersDb_ConstructionSiteRefConstructionSite",
                        column: x => x.ConstructionSiteRefConstructionSite,
                        principalTable: "LesChantiersDb",
                        principalColumn: "RefConstructionSite");
                });

            migrationBuilder.CreateTable(
                name: "ListeSolDb",
                columns: table => new
                {
                    RefSol = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    nomSol = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    zoneGeo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    famille = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClasseDuSol = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SousClasse = table.Column<int>(type: "int", nullable: false),
                    ClasseInférieure = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SousClasseInférieure = table.Column<int>(type: "int", nullable: false),
                    nomSolNormalise = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Dmax = table.Column<float>(type: "real", nullable: false),
                    Tamisat50mm = table.Column<float>(type: "real", nullable: false),
                    Tamisat2mm = table.Column<float>(type: "real", nullable: false),
                    tamisat80microns = table.Column<float>(type: "real", nullable: false),
                    VBS = table.Column<float>(type: "real", nullable: false),
                    Ip = table.Column<float>(type: "real", nullable: false),
                    Es = table.Column<float>(type: "real", nullable: false),
                    Phimu = table.Column<float>(type: "real", nullable: false),
                    ConstructionSiteRefConstructionSite = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ListeSolDb", x => x.RefSol);
                    table.ForeignKey(
                        name: "FK_ListeSolDb_LesChantiersDb_ConstructionSiteRefConstructionSite",
                        column: x => x.ConstructionSiteRefConstructionSite,
                        principalTable: "LesChantiersDb",
                        principalColumn: "RefConstructionSite");
                });

            migrationBuilder.CreateTable(
                name: "ListeSondeDb",
                columns: table => new
                {
                    RefSonde = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    NomSonde = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConstructionSiteRefConstructionSite = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ListeSondeDb", x => x.RefSonde);
                    table.ForeignKey(
                        name: "FK_ListeSondeDb_LesChantiersDb_ConstructionSiteRefConstructionSite",
                        column: x => x.ConstructionSiteRefConstructionSite,
                        principalTable: "LesChantiersDb",
                        principalColumn: "RefConstructionSite");
                });

            migrationBuilder.CreateTable(
                name: "ListeTestExpansionsDb",
                columns: table => new
                {
                    RefTest = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RefSonde = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ClassPrecis = table.Column<int>(type: "int", nullable: false),
                    typeTest = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    nomTest = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    dateTest = table.Column<DateTime>(type: "datetime2", nullable: false),
                    difPresEauAir = table.Column<float>(type: "real", nullable: false),
                    hauteurCpv = table.Column<float>(type: "real", nullable: false),
                    Di = table.Column<float>(type: "real", nullable: false),
                    Vc = table.Column<float>(type: "real", nullable: false),
                    Vs = table.Column<float>(type: "real", nullable: false),
                    EpMembrane = table.Column<float>(type: "real", nullable: false),
                    Prp = table.Column<float>(type: "real", nullable: false),
                    PrV = table.Column<float>(type: "real", nullable: false),
                    coefficientA = table.Column<float>(type: "real", nullable: false),
                    coefficientB = table.Column<float>(type: "real", nullable: false),
                    PrecisVc = table.Column<float>(type: "real", nullable: false),
                    PrecisVs = table.Column<float>(type: "real", nullable: false),
                    PrEpMembrane = table.Column<float>(type: "real", nullable: false),
                    PrecisA = table.Column<float>(type: "real", nullable: false),
                    PrecisB = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ListeTestExpansionsDb", x => x.RefTest);
                    table.ForeignKey(
                        name: "FK_ListeTestExpansionsDb_ListeSondeDb_RefSonde",
                        column: x => x.RefSonde,
                        principalTable: "ListeSondeDb",
                        principalColumn: "RefSonde");
                });

            migrationBuilder.CreateTable(
                name: "ListeTestResistancesDb",
                columns: table => new
                {
                    RefTest = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RefSonde = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ClassPrecis = table.Column<int>(type: "int", nullable: false),
                    nomTest = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    dateTest = table.Column<DateTime>(type: "datetime2", nullable: false),
                    typeTest = table.Column<float>(type: "real", nullable: false),
                    difPresEauAir = table.Column<float>(type: "real", nullable: false),
                    hauteurCpv = table.Column<float>(type: "real", nullable: false),
                    A0 = table.Column<float>(type: "real", nullable: false),
                    A1 = table.Column<float>(type: "real", nullable: false),
                    A2 = table.Column<float>(type: "real", nullable: false),
                    A3 = table.Column<float>(type: "real", nullable: false),
                    Pel = table.Column<float>(type: "real", nullable: false),
                    Pe = table.Column<float>(type: "real", nullable: false),
                    MySondeResistanceRefSonde = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    MesureCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ListeTestResistancesDb", x => x.RefTest);
                    table.ForeignKey(
                        name: "FK_ListeTestResistancesDb_ListeSondeDb_MySondeResistanceRefSonde",
                        column: x => x.MySondeResistanceRefSonde,
                        principalTable: "ListeSondeDb",
                        principalColumn: "RefSonde");
                    table.ForeignKey(
                        name: "FK_ListeTestResistancesDb_ListeSondeDb_RefSonde",
                        column: x => x.RefSonde,
                        principalTable: "ListeSondeDb",
                        principalColumn: "RefSonde");
                });

            migrationBuilder.CreateTable(
                name: "ListeMesureExpansionsDb",
                columns: table => new
                {
                    Ind = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RefTest = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IndiceMesure = table.Column<int>(type: "int", nullable: false),
                    Pression = table.Column<float>(type: "real", nullable: false),
                    Volume = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ListeMesureExpansionsDb", x => x.Ind);
                    table.ForeignKey(
                        name: "FK_ListeMesureExpansionsDb_ListeTestExpansionsDb_RefTest",
                        column: x => x.RefTest,
                        principalTable: "ListeTestExpansionsDb",
                        principalColumn: "RefTest",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ListeTestForageDb",
                columns: table => new
                {
                    RefTest = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RefConstructionSite = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RefForage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MySolRefSol = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    MaResistanceRefTest = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    MaExpansionRefTest = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    MonTestRefTest = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    refSonde = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    refTestResistancePropre = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    refTestExpansionPropre = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    refTestPerteDeCharge = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RefSol = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    nomTest = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    dateTest = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    heureTest = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Enregistreur = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Operateur = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VolSonde = table.Column<float>(type: "real", nullable: false),
                    ClassPrecis = table.Column<int>(type: "int", nullable: false),
                    PrecisVolSonde = table.Column<float>(type: "real", nullable: false),
                    difPresEauAir = table.Column<float>(type: "real", nullable: false),
                    hauteurCpv = table.Column<float>(type: "real", nullable: false),
                    presP0 = table.Column<float>(type: "real", nullable: false),
                    presFluage = table.Column<float>(type: "real", nullable: false),
                    presLimite = table.Column<float>(type: "real", nullable: false),
                    presLimiteNF = table.Column<float>(type: "real", nullable: false),
                    presLimiteTheo = table.Column<float>(type: "real", nullable: false),
                    PrecisVolume = table.Column<float>(type: "real", nullable: false),
                    PrecisPression = table.Column<float>(type: "real", nullable: false),
                    poidsVol = table.Column<float>(type: "real", nullable: false),
                    poidsVolDejauge = table.Column<float>(type: "real", nullable: false),
                    coefPoisson = table.Column<float>(type: "real", nullable: false),
                    modPressio = table.Column<float>(type: "real", nullable: false),
                    modPressioNf = table.Column<float>(type: "real", nullable: false),
                    modElastique = table.Column<float>(type: "real", nullable: false),
                    modElastiqueNf = table.Column<float>(type: "real", nullable: false),
                    Cohesion = table.Column<float>(type: "real", nullable: false),
                    Frottement = table.Column<float>(type: "real", nullable: false),
                    FrottementIntergranulaire = table.Column<float>(type: "real", nullable: false),
                    U0 = table.Column<float>(type: "real", nullable: false),
                    profNappe = table.Column<float>(type: "real", nullable: false),
                    profSonde = table.Column<float>(type: "real", nullable: false),
                    coteZeroGeomeca = table.Column<float>(type: "real", nullable: false),
                    EventsSuspendus = table.Column<bool>(type: "bit", nullable: false),
                    Pe = table.Column<float>(type: "real", nullable: false),
                    Ie = table.Column<int>(type: "int", nullable: false),
                    ConstructionSiteRefConstructionSite = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ForageRefForage = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ListeTestForageDb", x => x.RefTest);
                    table.ForeignKey(
                        name: "FK_ListeTestForageDb_Forages_ForageRefForage",
                        column: x => x.ForageRefForage,
                        principalTable: "Forages",
                        principalColumn: "RefForage");
                    table.ForeignKey(
                        name: "FK_ListeTestForageDb_LesChantiersDb_ConstructionSiteRefConstructionSite",
                        column: x => x.ConstructionSiteRefConstructionSite,
                        principalTable: "LesChantiersDb",
                        principalColumn: "RefConstructionSite");
                    table.ForeignKey(
                        name: "FK_ListeTestForageDb_ListeTestExpansionsDb_MaExpansionRefTest",
                        column: x => x.MaExpansionRefTest,
                        principalTable: "ListeTestExpansionsDb",
                        principalColumn: "RefTest");
                    table.ForeignKey(
                        name: "FK_ListeTestForageDb_ListeTestForageDb_MonTestRefTest",
                        column: x => x.MonTestRefTest,
                        principalTable: "ListeTestForageDb",
                        principalColumn: "RefTest");
                    table.ForeignKey(
                        name: "FK_ListeTestForageDb_ListeTestResistancesDb_MaResistanceRefTest",
                        column: x => x.MaResistanceRefTest,
                        principalTable: "ListeTestResistancesDb",
                        principalColumn: "RefTest");
                    table.ForeignKey(
                        name: "FK_ListeTestForageDb_ListeSolDb_MySolRefSol",
                        column: x => x.MySolRefSol,
                        principalTable: "ListeSolDb",
                        principalColumn: "RefSol");
                    table.ForeignKey(
                        name: "FK_ListeTestForageDb_ListeSondeDb_refSonde",
                        column: x => x.refSonde,
                        principalTable: "ListeSondeDb",
                        principalColumn: "RefSonde");
                });

            migrationBuilder.CreateTable(
                name: "ListeMesureResistanceDb",
                columns: table => new
                {
                    Ind = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RefTest = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IndiceMesure = table.Column<int>(type: "int", nullable: false),
                    Pression = table.Column<float>(type: "real", nullable: false),
                    Volume = table.Column<float>(type: "real", nullable: false),
                    TestResistanceRefTest = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ListeMesureResistanceDb", x => x.Ind);
                    table.ForeignKey(
                        name: "FK_ListeMesureResistanceDb_ListeTestResistancesDb_TestResistanceRefTest",
                        column: x => x.TestResistanceRefTest,
                        principalTable: "ListeTestResistancesDb",
                        principalColumn: "RefTest");
                    table.ForeignKey(
                        name: "FK_ListeMesureResistanceDb_ListeTestResistancesDb_RefTest",
                        column: x => x.RefTest,
                        principalTable: "ListeTestResistancesDb",
                        principalColumn: "RefTest",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ListeMesureTestForageDb",
                columns: table => new
                {
                    Ind = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RefTest = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IndiceMesure = table.Column<int>(type: "int", nullable: false),
                    Pression = table.Column<float>(type: "real", nullable: true),
                    Volume15 = table.Column<float>(type: "real", nullable: true),
                    Volume30 = table.Column<float>(type: "real", nullable: true),
                    Volume60 = table.Column<float>(type: "real", nullable: true),
                    ContrainteRadiale = table.Column<float>(type: "real", nullable: true),
                    ContrainteVerticale = table.Column<float>(type: "real", nullable: true),
                    Deformation = table.Column<float>(type: "real", nullable: true),
                    PNF = table.Column<float>(type: "real", nullable: true),
                    VNF = table.Column<float>(type: "real", nullable: true),
                    Qi = table.Column<float>(type: "real", nullable: true),
                    Pi = table.Column<float>(type: "real", nullable: true),
                    Qp = table.Column<float>(type: "real", nullable: true),
                    Pp = table.Column<float>(type: "real", nullable: true),
                    ConstructionSiteRefConstructionSite = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ListeMesureTestForageDb", x => x.Ind);
                    table.ForeignKey(
                        name: "FK_ListeMesureTestForageDb_LesChantiersDb_ConstructionSiteRefConstructionSite",
                        column: x => x.ConstructionSiteRefConstructionSite,
                        principalTable: "LesChantiersDb",
                        principalColumn: "RefConstructionSite");
                    table.ForeignKey(
                        name: "FK_ListeMesureTestForageDb_ListeTestForageDb_RefTest",
                        column: x => x.RefTest,
                        principalTable: "ListeTestForageDb",
                        principalColumn: "RefTest",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Forages_ConstructionSiteRefConstructionSite",
                table: "Forages",
                column: "ConstructionSiteRefConstructionSite");

            migrationBuilder.CreateIndex(
                name: "IX_ListeTestExpansionsDb_RefSonde",
                table: "ListeTestExpansionsDb",
                column: "RefSonde");

            migrationBuilder.CreateIndex(
                name: "IX_ListeTestForageDb_ConstructionSiteRefConstructionSite",
                table: "ListeTestForageDb",
                column: "ConstructionSiteRefConstructionSite");

            migrationBuilder.CreateIndex(
                name: "IX_ListeTestForageDb_ForageRefForage",
                table: "ListeTestForageDb",
                column: "ForageRefForage");

            migrationBuilder.CreateIndex(
                name: "IX_ListeTestForageDb_MaExpansionRefTest",
                table: "ListeTestForageDb",
                column: "MaExpansionRefTest");

            migrationBuilder.CreateIndex(
                name: "IX_ListeTestForageDb_MaResistanceRefTest",
                table: "ListeTestForageDb",
                column: "MaResistanceRefTest");

            migrationBuilder.CreateIndex(
                name: "IX_ListeTestForageDb_MonTestRefTest",
                table: "ListeTestForageDb",
                column: "MonTestRefTest");

            migrationBuilder.CreateIndex(
                name: "IX_ListeTestForageDb_MySolRefSol",
                table: "ListeTestForageDb",
                column: "MySolRefSol");

            migrationBuilder.CreateIndex(
                name: "IX_ListeTestForageDb_refSonde",
                table: "ListeTestForageDb",
                column: "refSonde");

            migrationBuilder.CreateIndex(
                name: "IX_ListeTestResistancesDb_MySondeResistanceRefSonde",
                table: "ListeTestResistancesDb",
                column: "MySondeResistanceRefSonde");

            migrationBuilder.CreateIndex(
                name: "IX_ListeTestResistancesDb_RefSonde",
                table: "ListeTestResistancesDb",
                column: "RefSonde");

            migrationBuilder.CreateIndex(
                name: "IX_ListeMesureTestForageDb_ConstructionSiteRefConstructionSite",
                table: "ListeMesureTestForageDb",
                column: "ConstructionSiteRefConstructionSite");

            migrationBuilder.CreateIndex(
                name: "IX_ListeMesureTestForageDb_RefTest",
                table: "ListeMesureTestForageDb",
                column: "RefTest");

            migrationBuilder.CreateIndex(
                name: "IX_ListeMesureExpansionsDb_RefTest",
                table: "ListeMesureExpansionsDb",
                column: "RefTest");

            migrationBuilder.CreateIndex(
                name: "IX_ListeMesureResistanceDb_TestResistanceRefTest",
                table: "ListeMesureResistanceDb",
                column: "TestResistanceRefTest");

            migrationBuilder.CreateIndex(
                name: "IX_ListeMesureResistanceDb_RefTest",
                table: "ListeMesureResistanceDb",
                column: "RefTest");

            migrationBuilder.CreateIndex(
                name: "IX_ListeSolDb_ConstructionSiteRefConstructionSite",
                table: "ListeSolDb",
                column: "ConstructionSiteRefConstructionSite");

            migrationBuilder.CreateIndex(
                name: "IX_ListeSondeDb_ConstructionSiteRefConstructionSite",
                table: "ListeSondeDb",
                column: "ConstructionSiteRefConstructionSite");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ListeMesureTestForageDb");

            migrationBuilder.DropTable(
                name: "ListeMesureExpansionsDb");

            migrationBuilder.DropTable(
                name: "ListeMesureResistanceDb");

            migrationBuilder.DropTable(
                name: "ListeTestForageDb");

            migrationBuilder.DropTable(
                name: "Forages");

            migrationBuilder.DropTable(
                name: "ListeTestExpansionsDb");

            migrationBuilder.DropTable(
                name: "ListeTestResistancesDb");

            migrationBuilder.DropTable(
                name: "ListeSolDb");

            migrationBuilder.DropTable(
                name: "ListeSondeDb");

            migrationBuilder.DropTable(
                name: "LesChantiersDb");
        }
    }
}
