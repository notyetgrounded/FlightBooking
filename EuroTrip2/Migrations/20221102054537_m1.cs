using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EuroTrip2.Migrations
{
    public partial class m1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Admins",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AdminPassword = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Admins", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Flights",
                columns: table => new
                {
                    flightId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    flightName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    seatCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Flights", x => x.flightId);
                });

            migrationBuilder.CreateTable(
                name: "Passengers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Age = table.Column<int>(type: "int", nullable: false),
                    Gender = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Passengers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Places",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IOTA = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Places", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Registrations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EmailID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Registrations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Seats",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Flight_Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Seats", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Seats_Flights_Flight_Id",
                        column: x => x.Flight_Id,
                        principalTable: "Flights",
                        principalColumn: "flightId");
                });

            migrationBuilder.CreateTable(
                name: "TripRoutes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Distance = table.Column<int>(type: "int", nullable: false),
                    Source_Id = table.Column<int>(type: "int", nullable: false),
                    Destination_Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TripRoutes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TripRoutes_Places_Destination_Id",
                        column: x => x.Destination_Id,
                        principalTable: "Places",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TripRoutes_Places_Source_Id",
                        column: x => x.Source_Id,
                        principalTable: "Places",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Trips",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RouteName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SourceTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DestinationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PassengerCount = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<int>(type: "int", nullable: false),
                    Flight_Id = table.Column<int>(type: "int", nullable: false),
                    TripRoute_Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trips", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Trips_Flights_Flight_Id",
                        column: x => x.Flight_Id,
                        principalTable: "Flights",
                        principalColumn: "flightId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Trips_TripRoutes_TripRoute_Id",
                        column: x => x.TripRoute_Id,
                        principalTable: "TripRoutes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Bookings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BookingDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TotalPrice = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Trip_Id = table.Column<int>(type: "int", nullable: false),
                    User_Id = table.Column<int>(type: "int", nullable: false),
                    NextBooking_Id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bookings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Bookings_Bookings_NextBooking_Id",
                        column: x => x.NextBooking_Id,
                        principalTable: "Bookings",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Bookings_Trips_Trip_Id",
                        column: x => x.Trip_Id,
                        principalTable: "Trips",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Bookings_Users_User_Id",
                        column: x => x.User_Id,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SeatStatuses",
                columns: table => new
                {
                    Seat_Id = table.Column<int>(type: "int", nullable: false),
                    Trip_Id = table.Column<int>(type: "int", nullable: false),
                    IsFree = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SeatStatuses", x => new { x.Seat_Id, x.Trip_Id });
                    table.ForeignKey(
                        name: "FK_SeatStatuses_Seats_Seat_Id",
                        column: x => x.Seat_Id,
                        principalTable: "Seats",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SeatStatuses_Trips_Trip_Id",
                        column: x => x.Trip_Id,
                        principalTable: "Trips",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Tickets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<int>(type: "int", nullable: false),
                    Seat_Id = table.Column<int>(type: "int", nullable: false),
                    Booking_Id = table.Column<int>(type: "int", nullable: false),
                    Passenger_Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tickets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tickets_Bookings_Booking_Id",
                        column: x => x.Booking_Id,
                        principalTable: "Bookings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Tickets_Passengers_Passenger_Id",
                        column: x => x.Passenger_Id,
                        principalTable: "Passengers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Tickets_Seats_Seat_Id",
                        column: x => x.Seat_Id,
                        principalTable: "Seats",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_NextBooking_Id",
                table: "Bookings",
                column: "NextBooking_Id",
                unique: true,
                filter: "[NextBooking_Id] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_Trip_Id",
                table: "Bookings",
                column: "Trip_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_User_Id",
                table: "Bookings",
                column: "User_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Seats_Flight_Id",
                table: "Seats",
                column: "Flight_Id");

            migrationBuilder.CreateIndex(
                name: "IX_SeatStatuses_Trip_Id",
                table: "SeatStatuses",
                column: "Trip_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_Booking_Id",
                table: "Tickets",
                column: "Booking_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_Passenger_Id",
                table: "Tickets",
                column: "Passenger_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_Seat_Id",
                table: "Tickets",
                column: "Seat_Id");

            migrationBuilder.CreateIndex(
                name: "IX_TripRoutes_Destination_Id",
                table: "TripRoutes",
                column: "Destination_Id");

            migrationBuilder.CreateIndex(
                name: "IX_TripRoutes_Source_Id",
                table: "TripRoutes",
                column: "Source_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Trips_Flight_Id",
                table: "Trips",
                column: "Flight_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Trips_TripRoute_Id",
                table: "Trips",
                column: "TripRoute_Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Admins");

            migrationBuilder.DropTable(
                name: "Registrations");

            migrationBuilder.DropTable(
                name: "SeatStatuses");

            migrationBuilder.DropTable(
                name: "Tickets");

            migrationBuilder.DropTable(
                name: "Bookings");

            migrationBuilder.DropTable(
                name: "Passengers");

            migrationBuilder.DropTable(
                name: "Seats");

            migrationBuilder.DropTable(
                name: "Trips");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Flights");

            migrationBuilder.DropTable(
                name: "TripRoutes");

            migrationBuilder.DropTable(
                name: "Places");
        }
    }
}
