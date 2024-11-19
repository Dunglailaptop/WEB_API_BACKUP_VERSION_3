using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace MyCinema.Model;

public partial class CinemaContext : DbContext
{
    public CinemaContext()
    {
    }

    public CinemaContext(DbContextOptions<CinemaContext> options): base(options)
    {
        try
        {
            var databaseCreator = Database.GetService<IDatabaseCreator>() as RelationalDatabaseCreator;
            if (databaseCreator != null)
            {
                if (!databaseCreator.CanConnect()) databaseCreator.Create();
                if (!databaseCreator.HasTables()) databaseCreator.CreateTables();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
    //INNNER JOIN START
    public DbSet<likeandcomment> likeandcomment { get; set; }
    public DbSet<CommentVideo> CommentVideo { get; set; }
    public DbSet<checkin> checkin { get; set; }
    public DbSet<ListFoodBillCombo> ListFoodBillCombo { get; set; }
    public DbSet<InterestWithRoom> InterestWithRooms { get; set; }
    public DbSet<paymentVNPAY> paymentVNPAY { get; set; }
    public DbSet<ReportFoodCombo> ReportFoodCombos { get; set; }
    public DbSet<ReportTicket> ReportTickets { get; set; }
    public DbSet<totalfoodcombowithbill> totalfoodcombowithbills { get; set; }
    public DbSet<ReportFoodComboALL> ReportFoodComboALLs { get; set; }
    public DbSet<ReportTicketALL> ReportTicketALLs { get; set; }
    public DbSet<ReportMovie> ReportMovies { get; set; }
    public DbSet<ReportFood> ReportFoods { get; set; }
    public DbSet<FoodComboWithBills> FoodComboWithBills { get; set; }
    public DbSet<FoodComboBill> FoodComboBill { get; set; }
    public DbSet<FoodCombo> Foodcombo { get; set; }
    public DbSet<CINEMA> CINEMAS { get; set; }
    public DbSet<USERS> USERS { get; set; }
    public DbSet<CHAIR> CHAIRS { get; set; }
    public DbSet<CHAIRDETAILBILL> CHAIRSDETAIL { get; set; }
    public DbSet<INTERESTCINEMA> INTERESTCINEMA { get; set; }
    public DbSet<LISTCINEMA> LISTCINEMA { get; set; }

    public DbSet<FoodCombillPayment> FoodCombillPayment { get; set; }

    public DbSet<ListFoodCombo> ListFoodCombo { get; set; }

    public DbSet<MoviesBooking> moviesBookings { get; set; }

    public DbSet<Notifaction> Notifaction { get; set; }
    //INNERHOIN END
    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<Bill> Bills { get; set; }

    public virtual DbSet<CategoryMovie> CategoryMovies { get; set; }

    public virtual DbSet<Categorychair> Categorychairs { get; set; }

    public virtual DbSet<Categoryfood> Categoryfoods { get; set; }

    public virtual DbSet<Chair> Chairs { get; set; }

    public virtual DbSet<Cinema> Cinemas { get; set; }

    public virtual DbSet<Cinemainterest> Cinemainterests { get; set; }

    public virtual DbSet<Comment> Comments { get; set; }

    public virtual DbSet<EfmigrationsHistory> EfmigrationsHistories { get; set; }

    public virtual DbSet<Food> Foods { get; set; }

    public virtual DbSet<Listcategorychair> Listcategorychairs { get; set; }

    public virtual DbSet<Listfoodbill> Listfoodbills { get; set; }

    public virtual DbSet<Movie> Movies { get; set; }

    public virtual DbSet<Nation> Nations { get; set; }

    public virtual DbSet<Problem> Problems { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Room> Rooms { get; set; }

    public virtual DbSet<Ticket> Tickets { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Userofcinema> Userofcinemas { get; set; }

    public virtual DbSet<Videouser> Videousers { get; set; }

    public virtual DbSet<Voucher> Vouchers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySQL("Server=localhost;Database=cinema;Uid=root;Pwd=2792001dung");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.Username).HasName("PRIMARY");

            entity.ToTable("accounts");

            entity.HasIndex(e => e.Idusers, "kp_account_user");

            entity.Property(e => e.Password).HasMaxLength(255);

            entity.HasOne(d => d.IdusersNavigation).WithMany(p => p.Accounts)
                .HasForeignKey(d => d.Idusers)
                .HasConstraintName("kp_account_user");
        });

        modelBuilder.Entity<Bill>(entity =>
        {
            entity.HasKey(e => e.Idbill).HasName("PRIMARY");

            entity.ToTable("Bill");

            entity.HasIndex(e => e.Idcinema, "kp_Bill_cinema");

            entity.HasIndex(e => e.Idinterest, "kp_Bill_interest");

            entity.HasIndex(e => e.Idmovie, "kp_Bill_movie");

            entity.HasIndex(e => e.Iduser, "kp_Bill_user");

            entity.HasIndex(e => e.Idvoucher, "kp_Bill_voucher");

            entity.Property(e => e.Datebill).HasColumnType("datetime");
            entity.Property(e => e.Note)
                .HasMaxLength(255)
                .HasColumnName("note");
            entity.Property(e => e.Quantityticket).HasColumnName("quantityticket");
            entity.Property(e => e.Statusbill).HasColumnName("statusbill");

            entity.HasOne(d => d.IdinterestNavigation).WithMany(p => p.Bills)
                .HasForeignKey(d => d.Idinterest)
                .HasConstraintName("kp_Bill_interest");

            entity.HasOne(d => d.IduserNavigation).WithMany(p => p.Bills)
                .HasForeignKey(d => d.Iduser)
                .HasConstraintName("kp_Bill_user");

            entity.HasOne(d => d.IdvoucherNavigation).WithMany(p => p.Bills)
                .HasForeignKey(d => d.Idvoucher)
                .HasConstraintName("kp_Bill_voucher");
        });

        modelBuilder.Entity<CategoryMovie>(entity =>
        {
            entity.HasKey(e => e.Idcategorymovie).HasName("PRIMARY");

            entity.ToTable("CategoryMovie");

            entity.Property(e => e.Namecategorymovie).HasMaxLength(100);
        });

        modelBuilder.Entity<Categorychair>(entity =>
        {
            entity.HasKey(e => e.Idcategorychair).HasName("PRIMARY");

            entity.ToTable("Categorychair");

            entity.Property(e => e.Colorchair).HasMaxLength(200);
            entity.Property(e => e.Namecategorychair).HasMaxLength(200);
            entity.Property(e => e.Price).HasColumnName("price");
        });

        modelBuilder.Entity<Categoryfood>(entity =>
        {
            entity.HasKey(e => e.Idcategoryfood).HasName("PRIMARY");

            entity.ToTable("Categoryfood");

            entity.Property(e => e.Namecategoryfood).HasMaxLength(255);
        });

        modelBuilder.Entity<Chair>(entity =>
        {
            entity.HasKey(e => e.Idchair).HasName("PRIMARY");

            entity.ToTable("chair");

            entity.HasIndex(e => e.Idcategorychair, "kp_chair_categorychair");

            entity.HasIndex(e => e.Idroom, "kp_chair_room");

            entity.Property(e => e.RowChar)
                .HasMaxLength(1)
                .IsFixedLength();

            entity.HasOne(d => d.IdcategorychairNavigation).WithMany(p => p.Chairs)
                .HasForeignKey(d => d.Idcategorychair)
                .HasConstraintName("kp_chair_categorychair");

            entity.HasOne(d => d.IdroomNavigation).WithMany(p => p.Chairs)
                .HasForeignKey(d => d.Idroom)
                .HasConstraintName("kp_chair_room");
        });

        modelBuilder.Entity<Cinema>(entity =>
        {
            entity.HasKey(e => e.Idcinema).HasName("PRIMARY");

            entity.ToTable("Cinema");

            entity.Property(e => e.Address).HasMaxLength(255);
            entity.Property(e => e.Describes).HasMaxLength(255);
            entity.Property(e => e.Namecinema).HasMaxLength(255);
            entity.Property(e => e.Phone)
                .HasMaxLength(11)
                .IsFixedLength();
            entity.Property(e => e.Picture).HasMaxLength(255);
        });

        modelBuilder.Entity<Cinemainterest>(entity =>
        {
            entity.HasKey(e => e.Idinterest).HasName("PRIMARY");

            entity.ToTable("cinemainterest");

            entity.HasIndex(e => e.Idmovie, "kp_cinemainterest_moive");

            entity.HasIndex(e => e.Idroom, "kp_cinemainterest_room");

            entity.Property(e => e.Dateshow).HasColumnType("date");
            entity.Property(e => e.Times).HasColumnType("datetime");

            entity.HasOne(d => d.IdmovieNavigation).WithMany(p => p.Cinemainterests)
                .HasForeignKey(d => d.Idmovie)
                .HasConstraintName("kp_cinemainterest_moive");

            entity.HasOne(d => d.IdroomNavigation).WithMany(p => p.Cinemainterests)
                .HasForeignKey(d => d.Idroom)
                .HasConstraintName("kp_cinemainterest_room");
        });

        modelBuilder.Entity<Comment>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("comments");

            entity.HasIndex(e => e.Iduser, "kp_comments_user");

            entity.HasIndex(e => e.Idvideo, "kp_comments_video");

            entity.Property(e => e.Idcomments).HasMaxLength(255);

            entity.HasOne(d => d.IduserNavigation).WithMany()
                .HasForeignKey(d => d.Iduser)
                .HasConstraintName("kp_comments_user");

            entity.HasOne(d => d.IdvideoNavigation).WithMany()
                .HasForeignKey(d => d.Idvideo)
                .HasConstraintName("kp_comments_video");
        });

        modelBuilder.Entity<EfmigrationsHistory>(entity =>
        {
            entity.HasKey(e => e.MigrationId).HasName("PRIMARY");

            entity.ToTable("__EFMigrationsHistory");

            entity.Property(e => e.MigrationId).HasMaxLength(150);
            entity.Property(e => e.ProductVersion).HasMaxLength(32);
        });

        modelBuilder.Entity<Food>(entity =>
        {
            entity.HasKey(e => e.Idfood).HasName("PRIMARY");

            entity.ToTable("food");

            entity.HasIndex(e => e.Idcategoryfood, "kp_food_categoryfood");

            entity.Property(e => e.Namefood).HasMaxLength(255);
            entity.Property(e => e.Picture)
                .HasMaxLength(255)
                .HasColumnName("picture");
            entity.Property(e => e.Pricefood).HasColumnName("pricefood");
            entity.Property(e => e.Quantityfood).HasColumnName("quantityfood");
        });

        modelBuilder.Entity<Listcategorychair>(entity =>
        {
            entity.HasKey(e => e.Idlistcategorychair).HasName("PRIMARY");

            entity.ToTable("listcategorychair");

            entity.HasIndex(e => e.Idcategory, "kp_listcategorychair_categorychair");

            entity.HasIndex(e => e.Idroom, "kp_listcategorychair_room");

            entity.HasOne(d => d.IdcategoryNavigation).WithMany(p => p.Listcategorychairs)
                .HasForeignKey(d => d.Idcategory)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("kp_listcategorychair_categorychair");

            entity.HasOne(d => d.IdroomNavigation).WithMany(p => p.Listcategorychairs)
                .HasForeignKey(d => d.Idroom)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("kp_listcategorychair_room");
        });

        modelBuilder.Entity<Listfoodbill>(entity =>
        {
            entity.HasKey(e => e.Idlistfood).HasName("PRIMARY");

            entity.ToTable("listfoodbill");

            entity.HasIndex(e => e.Idbill, "kp_listfoodbill_bill");

            entity.HasIndex(e => e.Idfood, "kp_listfoodbill_food");

            entity.HasOne(d => d.IdbillNavigation).WithMany(p => p.Listfoodbills)
                .HasForeignKey(d => d.Idbill)
                .HasConstraintName("kp_listfoodbill_bill");

            entity.HasOne(d => d.IdfoodNavigation).WithMany(p => p.Listfoodbills)
                .HasForeignKey(d => d.Idfood)
                .HasConstraintName("kp_listfoodbill_food");
        });

        modelBuilder.Entity<Movie>(entity =>
        {
            entity.HasKey(e => e.Idmovie).HasName("PRIMARY");

            entity.ToTable("Movie");

            entity.HasIndex(e => e.Idcategorymovie, "kp_movie_categorymovie");

            entity.HasIndex(e => e.Idnation, "kp_movie_nation");

            entity.HasIndex(e => e.Idvideo, "kp_movie_videouser");

            entity.Property(e => e.Author)
                .HasMaxLength(255)
                .HasColumnName("author");
            entity.Property(e => e.Describes).HasMaxLength(255);
            entity.Property(e => e.Namemovie).HasMaxLength(255);
            entity.Property(e => e.Poster)
                .HasMaxLength(255)
                .HasColumnName("poster");
            entity.Property(e => e.Yearbirthday).HasColumnType("date");

            entity.HasOne(d => d.IdcategorymovieNavigation).WithMany(p => p.Movies)
                .HasForeignKey(d => d.Idcategorymovie)
                .HasConstraintName("kp_movie_categorymovie");

            entity.HasOne(d => d.IdnationNavigation).WithMany(p => p.Movies)
                .HasForeignKey(d => d.Idnation)
                .HasConstraintName("kp_movie_nation");
        });

        modelBuilder.Entity<Nation>(entity =>
        {
            entity.HasKey(e => e.Idnation).HasName("PRIMARY");

            entity.ToTable("Nation");

            entity.Property(e => e.Namenation).HasMaxLength(100);
        });

        modelBuilder.Entity<Problem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("problem");

            entity.HasIndex(e => e.Idusers, "kp_problem_user");

            entity.Property(e => e.Describes).HasMaxLength(255);
            entity.Property(e => e.Picture).HasMaxLength(255);
            entity.Property(e => e.Title).HasMaxLength(255);

            entity.HasOne(d => d.IdusersNavigation).WithMany(p => p.Problems)
                .HasForeignKey(d => d.Idusers)
                .HasConstraintName("kp_problem_user");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Idrole).HasName("PRIMARY");

            entity.Property(e => e.IdName).HasMaxLength(100);
        });

        modelBuilder.Entity<Room>(entity =>
        {
            entity.HasKey(e => e.Idroom).HasName("PRIMARY");

            entity.ToTable("Room");

            entity.HasIndex(e => e.Idcinema, "fk_idcinema_cinemas");

            entity.Property(e => e.Nameroom).HasMaxLength(20);
            entity.Property(e => e.Statusroom).HasColumnName("statusroom");

            entity.HasOne(d => d.IdcinemaNavigation).WithMany(p => p.Rooms)
                .HasForeignKey(d => d.Idcinema)
                .HasConstraintName("fk_idcinema_cinemas");
        });

        modelBuilder.Entity<Ticket>(entity =>
        {
            entity.HasKey(e => e.Idticket).HasName("PRIMARY");

            entity.ToTable("ticket");

            entity.HasIndex(e => e.Idbill, "kp_ticket_bill");

            entity.HasIndex(e => e.Idchair, "kp_ticket_chair");

            entity.HasIndex(e => e.Idinterest, "kp_ticket_interest");

            entity.HasOne(d => d.IdbillNavigation).WithMany(p => p.Tickets)
                .HasForeignKey(d => d.Idbill)
                .HasConstraintName("kp_ticket_bill");

            entity.HasOne(d => d.IdchairNavigation).WithMany(p => p.Tickets)
                .HasForeignKey(d => d.Idchair)
                .HasConstraintName("kp_ticket_chair");

            entity.HasOne(d => d.IdinterestNavigation).WithMany(p => p.Tickets)
                .HasForeignKey(d => d.Idinterest)
                .HasConstraintName("kp_ticket_interest");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Idusers).HasName("PRIMARY");

            entity.HasIndex(e => e.Idrole, "kp_user_role");

            entity.Property(e => e.Avatar)
                .HasMaxLength(255)
                .HasColumnName("avatar");
            entity.Property(e => e.Birthday).HasColumnType("date");
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.Fullname).HasMaxLength(255);
            entity.Property(e => e.Phone)
                .HasMaxLength(11)
                .IsFixedLength();

            entity.HasOne(d => d.IdroleNavigation).WithMany(p => p.Users)
                .HasForeignKey(d => d.Idrole)
                .HasConstraintName("kp_user_role");
        });

        modelBuilder.Entity<Userofcinema>(entity =>
        {
            entity.HasKey(e => e.Iduserofcinema).HasName("PRIMARY");

            entity.ToTable("userofcinema");

            entity.HasIndex(e => e.Idcinema, "kp_userofcinema_cinema");

            entity.HasIndex(e => e.Iduser, "kp_userofcinema_user");

            entity.HasOne(d => d.IdcinemaNavigation).WithMany(p => p.Userofcinemas)
                .HasForeignKey(d => d.Idcinema)
                .HasConstraintName("kp_userofcinema_cinema");

            entity.HasOne(d => d.IduserNavigation).WithMany(p => p.Userofcinemas)
                .HasForeignKey(d => d.Iduser)
                .HasConstraintName("kp_userofcinema_user");
        });

        modelBuilder.Entity<Videouser>(entity =>
        {
            entity.HasKey(e => e.Idvideo).HasName("PRIMARY");

            entity.ToTable("videouser");

            entity.HasIndex(e => e.Iduser, "kp_videouser_users");

            entity.Property(e => e.Dateup).HasColumnType("datetime");
            entity.Property(e => e.Describes).HasMaxLength(255);
            entity.Property(e => e.Imageview).HasMaxLength(255);
            entity.Property(e => e.Titlevideo).HasMaxLength(255);
            entity.Property(e => e.Types).HasColumnName("types");
            entity.Property(e => e.Videofile).HasMaxLength(255);

            entity.HasOne(d => d.IduserNavigation).WithMany(p => p.Videousers)
                .HasForeignKey(d => d.Iduser)
                .HasConstraintName("kp_videouser_users");
        });

        modelBuilder.Entity<Voucher>(entity =>
        {
            entity.HasKey(e => e.Idvoucher).HasName("PRIMARY");

            entity.ToTable("voucher");

            entity.Property(e => e.Namevoucher).HasMaxLength(255);
            entity.Property(e => e.Note).HasMaxLength(255);
            entity.Property(e => e.Poster).HasMaxLength(255);
        });
        //INNER JOIN START
        modelBuilder.Entity<InterestWithRoom>()
     .HasNoKey();
        modelBuilder.Entity<INTERESTCINEMA>()
      .HasNoKey(); // Assuming 'Id' is the primary key property name.
        modelBuilder.Entity<LISTCINEMA>()
       .HasNoKey();
        OnModelCreatingPartial(modelBuilder);
        modelBuilder.Entity<CHAIR>()
 .HasNoKey();
        OnModelCreatingPartial(modelBuilder);
        modelBuilder.Entity<CHAIRDETAILBILL>()
 .HasNoKey();
        OnModelCreatingPartial(modelBuilder);
        modelBuilder.Entity<USERS>()
      .HasNoKey();
        OnModelCreatingPartial(modelBuilder);
        OnModelCreatingPartial(modelBuilder);
        modelBuilder.Entity<CINEMA>()
      .HasNoKey();
        OnModelCreatingPartial(modelBuilder);
        modelBuilder.Entity<ReportTicket>()
      .HasNoKey();
        OnModelCreatingPartial(modelBuilder);
        modelBuilder.Entity<ReportFoodCombo>()
      .HasNoKey();
        OnModelCreatingPartial(modelBuilder);
        modelBuilder.Entity<ReportTicketALL>()
      .HasNoKey();
        OnModelCreatingPartial(modelBuilder);
        modelBuilder.Entity<ReportFoodComboALL>()
      .HasNoKey();
        OnModelCreatingPartial(modelBuilder);
        modelBuilder.Entity<ReportMovie>()
      .HasNoKey();
        OnModelCreatingPartial(modelBuilder);
        modelBuilder.Entity<ReportFood>()
      .HasNoKey();
        OnModelCreatingPartial(modelBuilder);
        modelBuilder.Entity<totalfoodcombowithbill>()
      .HasNoKey();
        OnModelCreatingPartial(modelBuilder);
        modelBuilder.Entity<MoviesBooking>()
      .HasNoKey();
        modelBuilder.Entity<FoodCombo>()
        .HasKey(fc => fc.idcombo); // Assuming Id is the primary key property
        modelBuilder.Entity<FoodComboBill>()
         .HasKey(fc => fc.Id);
        modelBuilder.Entity<FoodComboWithBills>()
      .HasKey(fc => fc.IdBillfoodCombo);
        modelBuilder.Entity<FoodCombillPayment>()
     .HasKey(fc => fc.id);
        modelBuilder.Entity<ListFoodCombo>()
     .HasKey(fc => fc.id);
        modelBuilder.Entity<Notifaction>()
   .HasKey(fc => fc.idnotifaction);
        modelBuilder.Entity<checkin>()
   .HasKey(fc => fc.idcheckin);
        modelBuilder.Entity<ListFoodBillCombo>()
.HasKey(fc => fc.idlistfood);
        modelBuilder.Entity<CommentVideo>().HasKey(fc => fc.idcomment);
        modelBuilder.Entity<likeandcomment>().HasKey(fc => fc.idliekandcomment);
        modelBuilder.Entity<paymentVNPAY>().HasKey(fc => fc.idpaymentvnpay);
        //INNER JOIN END
    }
    //   protected override void OnModelCreating(ModelBuilder modelBuilder)
    //    {

    //   }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
