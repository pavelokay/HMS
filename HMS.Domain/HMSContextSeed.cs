using HMS.Domain.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMS.Domain
{
    public class HMSContextSeed
    {
        public static async Task SeedAsync(HMSContext context, ILoggerFactory loggerFactory, int? retry = 0)
        {
            int retryForAvailability = retry.Value;

            try
            {
                await SeedHotelComplexesAsync(context);
                await SeedDepartmentsAsync(context);
                await SeedServicesAsync(context);
                await SeedHotelsAsync(context);
                await SeedRoomTypesAsync(context);
                await SeedRoomStatusesAsync(context);
                await SeedRoomsAsync(context);
                await SeedReservationStatusesAsync(context);
                await SeedReservationTypesAsync(context);
                await SeedTransactionStatusesAsync(context);
                await SeedPaymentTypesAsync(context);
                await SeedGendersAsync(context);
            }
            catch (Exception ex)
            {
                if (retryForAvailability < 3)
                {
                    retryForAvailability++;
                    var log = loggerFactory.CreateLogger<HMSContextSeed>();
                    log.LogError(ex.Message);
                    await SeedAsync(context, loggerFactory, retryForAvailability);
                }
                throw;
            }
        }

        private static async Task SeedHotelComplexesAsync(HMSContext context)
        {
            if (context.HotelComplexes.Any())
            {
                return;
            }
            context.HotelComplexes.AddRange(
                        new HotelComplex
                        {
                            Name = "January"
                        });
            await context.SaveChangesAsync();
            #region HotelComplexImages
            context.HotelComplexImages.AddRange(
                new HotelComplexImage
                {
                    HotelComplexId = 1,
                    AltName = "mainImage",
                    ImageFile = "main-photo.jpg"
                },
                new HotelComplexImage
                {
                    HotelComplexId = 1,
                    AltName = "hotelComplexImage1",
                    ImageFile = "HMS-hotels-photo-1.jpg"
                },
                new HotelComplexImage
                {
                    HotelComplexId = 1,
                    AltName = "hotelComplexImage2",
                    ImageFile = "HMS-hotels-photo-2.jpg"
                },
                new HotelComplexImage
                {
                    HotelComplexId = 1,
                    AltName = "hotelComplexImage3",
                    ImageFile = "HMS-hotels-photo-3.jpg"
                },
                new HotelComplexImage
                {
                    HotelComplexId = 1,
                    AltName = "hotelComplexImage4",
                    ImageFile = "HMS-hotels-photo-4.jpg"
                },
                new HotelComplexImage
                {
                    HotelComplexId = 1,
                    AltName = "hotelComplexImage5",
                    ImageFile = "HMS-hotels-photo-5.jpg"
                },
                new HotelComplexImage
                {
                    HotelComplexId = 1,
                    AltName = "hotelComplexImage6",
                    ImageFile = "HMS-hotels-photo-6.jpg"
                },
                new HotelComplexImage
                {
                    HotelComplexId = 1,
                    AltName = "hotelComplexSlideImage1",
                    ImageFile = "slider-img-1.jpg"
                },
                new HotelComplexImage
                {
                    HotelComplexId = 1,
                    AltName = "hotelComplexSlideImage2",
                    ImageFile = "slider-img-2.jpg"
                },
                new HotelComplexImage
                {
                    HotelComplexId = 1,
                    AltName = "hotelComplexSlideImage3",
                    ImageFile = "slider-img-3.jpg"
                },
                new HotelComplexImage
                {
                    HotelComplexId = 1,
                    AltName = "hotelComplexSlideImage4",
                    ImageFile = "slider-img-4.jpg"
                }
                );
            #endregion
            await context.SaveChangesAsync();
        }

        private static async Task SeedHotelsAsync(HMSContext context)
        {
            if (context.Hotels.Any())
            {
                return;
            }
            context.Hotels.AddRange(
                         new Hotel
                         {
                             Name = "KMS Hotel",
                             HotelComplexId = 1,
                             Country = "Россия",
                             Town = "Комсомольск-на-Амуре",
                             Address = "Ул. Ленина 27",
                             PhoneNumber = "54232356",
                             Fax = "8-812-1285567",
                             Email = "January.KmsHotel@gmail.com",
                             Site = "Hotel/KMSHotel",
                             Stars = 3,
                             FloorCount = 3,
                             Departments = context.Departments.ToList(),
                             Services = context.Services.ToList(),
                             Description = "Современная трехзвездочная гостиница, расположенная на улице Ленина, в престижном районе города, рядом с гостиницей распологается парк с незабываемыми видами"
                         },
                        new Hotel
                        {
                            Name = "Khabarovsk Hotel",
                            HotelComplexId = 1,
                            Country = "Россия",
                            Town = "Хабаровск",
                            Address = "Ул. Тургенева 15",
                            PhoneNumber = "54638156",
                            Fax = "8-813-163567",
                            Email = "January.KhabarovksHotel@gmail.com",
                            Site = "Hotel/KhabarovskHotel",
                            Stars = 4,
                            FloorCount = 15,
                            Departments = context.Departments.ToList(),
                            Services = context.Services.ToList(),
                            Description = "Современная четырехзвездочная гостиница, расположенная на улице Тургенева, в бизнес районе города, рядом с гостиницей распологается набережная с незабываемыми видами, а также парк"
                        },
                        new Hotel
                        {
                            Name = "Moscow Hotel",
                            HotelComplexId = 1,
                            Country = "Россия",
                            Town = "Москва",
                            Address = "Ул. Краснопресненская набережная 3",
                            PhoneNumber = "59235356",
                            Fax = "8-815-526567",
                            Email = "January.MoscowHotel@gmail.com",
                            Site = "Hotel/MoscowHotel",
                            Stars = 5,
                            FloorCount = 25,
                            Departments = context.Departments.ToList(),
                            Services = context.Services.ToList(),
                            Description = "Роскошный пятизвездочный отель, расположенный в деловом райноне Moscow City, рядом со зданием отеля расположены торговые центры, развлекательные заведения и зоны отдыха."
                        }
                        );
            await context.SaveChangesAsync();
            #region HotelImages
            context.HotelImages.AddRange(
                    new HotelImage
                    {
                        ImageFile = "KMSHotelImage1.jpg",
                        AltName = "mainImage",
                        HotelId = 1
                    },
                    new HotelImage
                    {
                        ImageFile = "KMSHotelImage2.jpg",
                        AltName = "hotelImage2",
                        HotelId = 1
                    },
                    new HotelImage
                    {
                        ImageFile = "KMSHotelImage3.jpg",
                        AltName = "hotelImage3",
                        HotelId = 1
                    },
                    new HotelImage
                    {
                        ImageFile = "KMSHotelImage4.jpg",
                        AltName = "hotelImage4",
                        HotelId = 1
                    },
                    new HotelImage
                    {
                        ImageFile = "KhabarovskHotelImage1.jpg",
                        AltName = "mainImage",
                        HotelId = 2
                    },
                    new HotelImage
                    {
                        ImageFile = "KhabarovskHotelImage2.jpg",
                        AltName = "hotelImage2",
                        HotelId = 2
                    },
                    new HotelImage
                    {
                        ImageFile = "KhabarovskHotelImage3.jpg",
                        AltName = "hotelImage3",
                        HotelId = 2
                    },
                    new HotelImage
                    {
                        ImageFile = "KhabarovskHotelImage4.jpg",
                        AltName = "hotelImage4",
                        HotelId = 2
                    },
                    new HotelImage
                    {
                        ImageFile = "MoscowHotelImage1.jpg",
                        AltName = "mainImage",
                        HotelId = 3
                    },
                    new HotelImage
                    {
                        ImageFile = "MoscowHotelImage2.jpg",
                        AltName = "hotelImage2",
                        HotelId = 3
                    },
                    new HotelImage
                    {
                        ImageFile = "MoscowHotelImage3.jpg",
                        AltName = "hotelImage3",
                        HotelId = 3
                    },
                    new HotelImage
                    {
                        ImageFile = "MoscowHotelImage4.jpg",
                        AltName = "hotelImage4",
                        HotelId = 3
                    }
                    );
            #endregion
            await context.SaveChangesAsync();
        }
        private static async Task SeedRoomTypesAsync(HMSContext context)
        {
            if (context.RoomTypes.Any())
            {
                return;
            }
            context.RoomTypes.AddRange(
                         new RoomType
                         {
                             Name = "Люкс Opera",
                             HotelId = 3,
                             Description = "Наблюдайте за сменой цветов неба над Москвой в атмосфере непревзойденного комфорта, нежась в кровати «King Size» с подсвечиваемым подиумом. Просторная спальня отделена от гостиной дверью, чтобы ничто не могло потревожить ваш сон",
                             Rate = 1020m,
                             Size = 76.86m,
                             Decor = "Люксы Opera обставлены эксклюзивной мебелью, которая украсила бы любую частную резиденцию. К вашим услугам собранная вручную кровать «King Size» с подсвечиваемым подиумом, солидный письменный стол из беленого дуба, мебель из японского ясеня тамо и отделанная белым кленом гримерная с изготовленными по индивидуальному заказу зеркалами и креслами",
                             Bathroom = "Просторная ванная, отделанная мрамором Брекчия Оничиато, с джакузи, отдельной душевой кабиной со стеклянными стенками и телевизором",
                             Bed = "Кровать «King Size», Раскладная кровать",
                             Features = "Отдельная гостиная и спальня. Панорамные окна с головокружительными видами на город. Современная техника, в том числе телевизор с изогнутым экраном с диагональю 140. Ступенчатые потолки с настраиваемой подсветкой. Глубокая ванна с джакузи,наполняемая водой за две минуты. Гардеробная. Отделка роскошными тканями",
                             Rating = 0m,
                             View = "Виды на город",
                             MaxGuest = 3,
                         },
                            new RoomType
                            {
                                Name = "Люкс Moscow City",
                                HotelId = 3,
                                Description = "Эти роскошные люксы с просторной гостиной, отдельной ванной комнатой для гостей и головокружительными видами на легендарный небоскреб Крайслер-билдинг идеально подходят для частных вечеринок",
                                Rate = 1360m,
                                Size = 96.12m,
                                Decor = "Люксы Moscow City обставлены эксклюзивной мебелью, которая украсила бы любую частную резиденцию. К вашим услугам собранная вручную кровать «King Size» с подсвечиваемым подиумом, солидный письменный стол из беленого дуба, люстра Artemide, шагреневый туалетный столик, ковры Tai Ping ручной работы, под которыми скрывается паркет из орехового дерева, и мебель из беленого дуба и японского ясеня тамо",
                                Bathroom = "Просторная ванная, отделанная мрамором Брекчия Оничиато, с джакузи, отдельной душевой кабиной со стеклянными стенками, телевизором и ванная комната для гостей",
                                Bed = "Кровать «King Size», Детская кровать или раскладная кровать",
                                Features = "Отдельная гостиная и спальня. Ступенчатые потолки с настраиваемой подсветкой. Глубокая ванна с джакузи,наполняемая водой за две минуты. Гардеробная. Отделка роскошными тканями",
                                Rating = 0m,
                                View = "Панорамные виды на город",
                                MaxGuest = 4,
                            },
                            new RoomType
                            {
                                Name = "Люкс Park",
                                HotelId = 3,
                                Description = "Из окон люкс номеров Park открываются совершенно головокружительные виды. А благодаря просторной гардеробной и ванной, напоминающей личный спа, вы узнаете, что такое по-настоящему высокий уровень комфорта",
                                Rate = 1140m,
                                Size = 79.15m,
                                Decor = "Люксы с видом на парк обставлены эксклюзивной мебелью, которая украсила бы любую частную резиденцию. К вашим услугам собранная вручную кровать «King Size» с подсвечиваемым подиумом, солидный письменный стол из беленого дуба, мебель из японского ясеня тамо и отделанная белым кленом гримерная с изготовленными по индивидуальному заказу зеркалами и креслами.",
                                Bathroom = "Просторная ванная, отделанная мрамором Брекчия Оничиато, с джакузи, отдельной душевой кабиной со стеклянными стенками и телевизором",
                                Bed = "Кровать «King Size», Раскладная кровать",
                                Features = "Отдельная гостиная и спальня. Панорамные окна с головокружительными видами на город.  Ступенчатые потолки с настраиваемой подсветкой. Глубокая ванна с джакузи,наполняемая водой за две минуты. Гардеробная. Отделка роскошными тканями",
                                Rating = 0m,
                                View = "Виды на парк",
                                MaxGuest = 3,
                            },
                            new RoomType
                            {
                                Name = "Studio 51",
                                HotelId = 3,
                                Description = "Откройте шторы и почувствуйте бешеный ритм жизни Мидтауна. Номер-студия — это просторные апартаменты с ванной в стиле спа и потрясающими видами на небоскребы из каждого окна",
                                Rate = 620m,
                                Size = 26.35m,
                                Decor = "Элегантный интерьер с эксклюзивной мебелью, дубовым письменным столом и ванной, отделанной итальянским мрамором",
                                Bathroom = "Ванная, отделанная мрамором Брекчия Оничиато, с душевой кабиной со стеклянными стенками и телевизором",
                                Bed = "Кровать «King Size»",
                                Features = "Из огромных окон открывается великолепный виды на город, парк и Moccow City",
                                Rating = 0m,
                                View = "Виды на город",
                                MaxGuest = 2,
                            },
                            new RoomType
                            {
                                Name = "Люкс Opera с терассой",
                                HotelId = 3,
                                Description = "В этом просторном люксе вас ждет нечто большее, чем просто домашний комфорт, — ванная, отделанная итальянским мрамором, кровать с подсвечиваемым подиумом и частная терраса, откуда открываются живописные виды на город",
                                Rate = 1120m,
                                Size = 56.65m,
                                Decor = "Люксы Opera с террасой обставлены эксклюзивной мебелью, которая украсила бы любую частную резиденцию. К вашим услугам собранная вручную кровать «King Size» с подсвечиваемым подиумом, солидный письменный стол из беленого дуба, мебель из японского ясеня тамо и отделанная белым кленом гримерная с изготовленными по индивидуальному заказу зеркалами и креслами",
                                Bathroom = "Просторная ванная, отделанная мрамором Брекчия Оничиато, с джакузи, отдельной душевой кабиной со стеклянными стенками и телевизором",
                                Bed = "Кровать «King Size», Две двуспальные кровати",
                                Features = "Полностью благоустроенная терраса площадью от 14 до 37 кв. м. Гардеробная. Отделка роскошными тканями",
                                Rating = 0m,
                                View = "Виды на Москву",
                                MaxGuest = 4,
                            },
                            new RoomType
                            {
                                Name = "Люкс Individual",
                                HotelId = 2,
                                Description = "Люксы Individual предостовляют все необходимое для комфортного пребывания и спокойного сна",
                                Rate = 230m,
                                Size = 24.15m,
                                Decor = "Однокомнатный номер, оснащенный современной удобной мебелью. Тщательно подобранный интерьер создает атмосферу уюта и комфорта для плодотворной работы или беспечного отдыха ",
                                Bathroom = "Просторная ванная с душем",
                                Bed = "Одна двуспальная кровать",
                                Features = " Панорамные окна с головокружительными видами на город.",
                                Rating = 0m,
                                View = "Виды на Хабаровск",
                                MaxGuest = 1,
                            },
                            new RoomType
                            {
                                Name = "Апартаменты Energy",
                                HotelId = 2,
                                Description = "Апартаменты Energy это роскошный комфортабельный двухкомнатный номер, состоящий из гостиной, спальни, ванной комнаты и гостевого туалета. Из окон номера открывается завораживающий вид на город",
                                Rate = 860m,
                                Size = 58.72m,
                                Decor = "Двухкомнатный номер имеет роскошный диван, 2 кожаных кресла, журнальный столик и гардероб",
                                Bathroom = "Spa ванная, гостевой туалет",
                                Bed = "Двухспальная кровать Twin",
                                Features = "Просторная гардеробная. туалетный столик. система центрального кондиционирования",
                                Rating = 0m,
                                View = "Виды на город",
                                MaxGuest = 3,
                            },
                            new RoomType
                            {
                                Name = "Studio 23",
                                HotelId = 2,
                                Description = "Уютный однокомнатный номер",
                                Rate = 110m,
                                Size = 18.12m,
                                Decor = "Журнальный столик, односпальная кровать",
                                Bathroom = "Ванная компата оборудованная душем",
                                Bed = "Односпальная кровать",
                                Features = "Кондиционер. Универсальный стол. Гардеробный шкаф",
                                Rating = 0m,
                                View = "Виды на городской парк",
                                MaxGuest = 1,
                            },
                            new RoomType
                            {
                                Name = "Люкс Business",
                                HotelId = 1,
                                Description = "Гости, пребывающие в городе по рабочим задачам оценят люксы Business, где они смогут спокойно отдахнуть после тяжелого дня",
                                Rate = 130m,
                                Size = 26.94m,
                                Decor = "Люкс оснащен современной мебелью. Тщательно подобранный интерьер создает атмосферу уюта и комфорта для плодотворной работы или беспечного отдыха",
                                Bathroom = "Собственная ванная комната укомплектована феном и бесплатными туалетно-косметическими принадлежностями",
                                Bed = "Двухспальная кровать",
                                Features = "Кондиционер. Холодильник. Гардеробный шкаф",
                                Rating = 0m,
                                View = "Из окна этого светлого номера бизнес-класса, открывается панорамный вид на город",
                                MaxGuest = 1,
                            },
                            new RoomType
                            {
                                Name = "Люкс Family",
                                HotelId = 1,
                                Description = "Гости отдыхающие всей семьей, оценят люксы Family с большой площадью и всеми удобствами номеров категории люкс",
                                Rate = 250m,
                                Size = 45.47m,
                                Decor = "В отделке интерьера использованы натуральные материалы.",
                                Bathroom = "Глубокая ванная с душем",
                                Bed = "Две двухспальный кровати",
                                Features = "Гардеробная. Отделка натуральными тканями",
                                Rating = 0m,
                                View = "Виды на городской парк",
                                MaxGuest = 3,
                            },
                            new RoomType
                            {
                                Name = "Люкс Presidental",
                                HotelId = 1,
                                Description = "Полностью обновленный комфортабельный двухкомнатный номер состоит из гостиной, спальни, гостевой и личной ванной комнаты",
                                Rate = 270m,
                                Size = 87.51m,
                                Decor = "В отделке интерьера использованы премиальные натуральные материалы",
                                Bathroom = "Ванная Спа. Отдельная гостевая ванная",
                                Bed = "Двухспальная кровать",
                                Features = "Отдельная гостиная и спальня. Окна с видами на город. Гардеробная. Отделка премиальными тканями",
                                Rating = 0m,
                                View = "Виды на город",
                                MaxGuest = 4,
                            });
            await context.SaveChangesAsync();
            #region RoomTypeImages
            context.RoomTypeImages.AddRange(
                    new RoomTypeImage
                    {
                        RoomTypeId = 9,
                        ImageFile = "MoscowHotelSuiteOpera1.jpg",
                        AltName = "mainImage"
                    },
                    new RoomTypeImage
                    {
                        RoomTypeId = 9,
                        ImageFile = "MoscowHotelSuiteOpera2.jpg",
                        AltName = "RoomTypeImage"
                    },
                    new RoomTypeImage
                    {
                        RoomTypeId = 9,
                        ImageFile = "MoscowHotelSuiteOpera3.jpg",
                        AltName = "RoomTypeImage"
                    },
                    new RoomTypeImage
                    {
                        RoomTypeId = 5,
                        ImageFile = "MoscowHotelSuiteMoscowCity1.jpg",
                        AltName = "mainImage"
                    },
                    new RoomTypeImage
                    {
                        RoomTypeId = 5,
                        ImageFile = "MoscowHotelSuiteMoscowCity2.jpg",
                        AltName = "RoomTypeImage"
                    },
                    new RoomTypeImage
                    {
                        RoomTypeId = 5,
                        ImageFile = "MoscowHotelSuiteMoscowCity3.jpg",
                        AltName = "RoomTypeImage"
                    },
                    new RoomTypeImage
                    {
                        RoomTypeId = 7,
                        ImageFile = "MoscowHotelSuitePark1.jpg",
                        AltName = "mainImage"
                    },
                    new RoomTypeImage
                    {
                        RoomTypeId = 7,
                        ImageFile = "MoscowHotelSuitePark2.jpg",
                        AltName = "RoomTypeImage"
                    },
                    new RoomTypeImage
                    {
                        RoomTypeId = 7,
                        ImageFile = "MoscowHotelSuitePark3.jpg",
                        AltName = "RoomTypeImage"
                    },
                    new RoomTypeImage
                    {
                        RoomTypeId = 6,
                        ImageFile = "MoscowHotelSuiteStudio1.jpg",
                        AltName = "mainImage"
                    },
                    new RoomTypeImage
                    {
                        RoomTypeId = 6,
                        ImageFile = "MoscowHotelSuiteStudio2.jpg",
                        AltName = "RoomTypeImage"
                    },
                    new RoomTypeImage
                    {
                        RoomTypeId = 6,
                        ImageFile = "MoscowHotelSuiteStudio3.jpg",
                        AltName = "RoomTypeImage"
                    },
                    new RoomTypeImage
                    {
                        RoomTypeId = 10,
                        ImageFile = "MoscowHotelSuiteOperaTerrace1.jpg",
                        AltName = "mainImage"
                    },
                    new RoomTypeImage
                    {
                        RoomTypeId = 10,
                        ImageFile = "MoscowHotelSuiteOperaTerrace2.jpg",
                        AltName = "RoomTypeImage"
                    },
                    new RoomTypeImage
                    {
                        RoomTypeId = 10,
                        ImageFile = "MoscowHotelSuiteOperaTerrace3.jpg",
                        AltName = "RoomTypeImage"
                    },
                    new RoomTypeImage
                    {
                        RoomTypeId = 4,
                        ImageFile = "KhabarovskHotelSuiteIndividual1.jpg",
                        AltName = "mainImage"
                    },
                    new RoomTypeImage
                    {
                        RoomTypeId = 4,
                        ImageFile = "KhabarovskHotelSuiteIndividual2.jpg",
                        AltName = "RoomTypeImage"
                    },
                    new RoomTypeImage
                    {
                        RoomTypeId = 4,
                        ImageFile = "KhabarovskHotelSuiteIndividual3.jpg",
                        AltName = "RoomTypeImage"
                    },
                    new RoomTypeImage
                    {
                        RoomTypeId = 3,
                        ImageFile = "KhabarovskHotelSuiteEnergy1.jpg",
                        AltName = "mainImage"
                    },
                    new RoomTypeImage
                    {
                        RoomTypeId = 3,
                        ImageFile = "KhabarovskHotelSuiteEnergy2.jpg",
                        AltName = "RoomTypeImage"
                    },
                    new RoomTypeImage
                    {
                        RoomTypeId = 3,
                        ImageFile = "KhabarovskHotelSuiteEnergy3.jpg",
                        AltName = "RoomTypeImage"
                    },
                    new RoomTypeImage
                    {
                        RoomTypeId = 2,
                        ImageFile = "KhabarovskHotelSuiteStudio1.jpg",
                        AltName = "mainImage"
                    },
                    new RoomTypeImage
                    {
                        RoomTypeId = 2,
                        ImageFile = "KhabarovskHotelSuiteStudio2.jpg",
                        AltName = "RoomTypeImage"
                    },
                    new RoomTypeImage
                    {
                        RoomTypeId = 2,
                        ImageFile = "KhabarovskHotelSuiteStudio3.jpg",
                        AltName = "RoomTypeImage"
                    },
                    new RoomTypeImage
                    {
                        RoomTypeId = 1,
                        ImageFile = "KMSHotelSuiteBusiness1.jpg",
                        AltName = "mainImage"
                    },
                    new RoomTypeImage
                    {
                        RoomTypeId = 1,
                        ImageFile = "KMSHotelSuiteBusiness2.jpg",
                        AltName = "RoomTypeImage"
                    },
                    new RoomTypeImage
                    {
                        RoomTypeId = 1,
                        ImageFile = "KMSHotelSuiteBusiness3.jpg",
                        AltName = "RoomTypeImage"
                    },
                    new RoomTypeImage
                    {
                        RoomTypeId = 8,
                        ImageFile = "KMSHotelSuiteFamily1.jpg",
                        AltName = "mainImage"
                    },
                    new RoomTypeImage
                    {
                        RoomTypeId = 8,
                        ImageFile = "KMSHotelSuiteFamily2.jpg",
                        AltName = "RoomTypeImage"
                    },
                    new RoomTypeImage
                    {
                        RoomTypeId = 8,
                        ImageFile = "KMSHotelSuiteFamily3.jpg",
                        AltName = "RoomTypeImage"
                    },
                    new RoomTypeImage
                    {
                        RoomTypeId = 11,
                        ImageFile = "KMSHotelSuitePresidental1.jpg",
                        AltName = "mainImage"
                    },
                    new RoomTypeImage
                    {
                        RoomTypeId = 11,
                        ImageFile = "KMSHotelSuitePresidental2.jpg",
                        AltName = "RoomTypeImage"
                    },
                    new RoomTypeImage
                    {
                        RoomTypeId = 11,
                        ImageFile = "KMSHotelSuitePresidental3.jpg",
                        AltName = "RoomTypeImage"
                    }
                    );
            #endregion
            await context.SaveChangesAsync();
        }
        private static async Task SeedRoomStatusesAsync(HMSContext context)
        {
            if (context.RoomStatuses.Any())
            {
                return;
            }
            context.RoomStatuses.AddRange(
                new RoomStatus
                {
                    Name = "Свободна",
                    Description = "The room is currently in no use"
                },
                new RoomStatus
                {
                    Name = "Занята",
                    Description = "The room is currently in use"
                },
                new RoomStatus
                {
                    Name = "Уборка",
                    Description = "Cleaning in the room"
                }
                );
            await context.SaveChangesAsync();
        }
        private static async Task SeedRoomsAsync(HMSContext context)
        {
            if (context.Rooms.Any())
            {
                return;
            }
            int roomTypeId = 1;
            //KMS HOTEL ROOMS
            for (int i = 1; i < 4; i++)
            {
                if (i == 1)
                {
                    roomTypeId = 1;
                }
                if (i == 2)
                {
                    roomTypeId = 8;
                }
                if (i == 3)
                {
                    roomTypeId = 11;
                }
                for (int j = 1; j < 6; j++)
                {

                    context.Rooms.Add(
                        new Room
                        {
                            FloorNumber = i,
                            Number = (i * 100) + (i + j),
                            RoomTypeId = roomTypeId,
                            RoomStatusId = 3
                        });
                }
            }
            //KHABAROVSK HOTEL ROOMS
            for (int i = 1; i < 16; i++)
            {
                if (i > 0 && i < 7)
                {
                    roomTypeId = 2;
                }
                if (i > 6 && i < 12)
                {
                    roomTypeId = 4;
                }
                if (i > 11)
                {
                    roomTypeId = 3;
                }
                for (int j = 1; j < 6; j++)
                {

                    context.Rooms.Add(
                        new Room
                        {
                            FloorNumber = i,
                            Number = (i * 100) + (i + j),
                            RoomTypeId = roomTypeId,
                            RoomStatusId = 3
                        });
                }
            }
            //MOSCOW HOTEL ROOMS
            for (int i = 1; i < 26; i++)
            {
                if (i > 0 && i < 8)
                {
                    roomTypeId = 6;
                }
                if (i > 7 && i < 14)
                {
                    roomTypeId = 9;
                }
                if (i > 13 && i < 18)
                {
                    roomTypeId = 10;
                }
                if (i > 17 && i < 22)
                {
                    roomTypeId = 7;
                }
                if (i > 21)
                {
                    roomTypeId = 5;
                }
                for (int j = 1; j < 6; j++)
                {

                    context.Rooms.Add(
                        new Room
                        {
                            FloorNumber = i,
                            Number = (i * 100) + (i + j),
                            RoomTypeId = roomTypeId,
                            RoomStatusId = 3
                        });
                }
            }
            await context.SaveChangesAsync();
        }
        private static async Task SeedDepartmentsAsync(HMSContext context)
        {
            if (context.Departments.Any())
            {
                return;
            }
            context.Departments.AddRange(
                new Department
                {
                    Name = "Accomodation",
                    Description = "Accomodation department of hotel"
                },
                new Department
                {
                    Name = "Servant",
                    Description = "Servant department of hotel"
                },
                new Department
                {
                    Name = "Maintenance",
                    Description = "Maintenance department of hotel"
                },
                new Department
                {
                    Name = "Admin",
                    Description = "Admin department of hotel"
                },
                new Department
                {
                    Name = "Accountant",
                    Description = "Accountant department of hotel"
                }
                );
            await context.SaveChangesAsync();
        }
        private static async Task SeedServicesAsync(HMSContext context)
        {
            if (context.Services.Any())
            {
                return;
            }
            context.Services.AddRange(
                new Service
                {
                    Name = "Чистка одежды",
                    Description = "Сдайте одежду в прачечную отеля и через 3 часа вам принесут ваши вещи в отличном состоянии",
                    Price = 7m
                },
                new Service
                {
                    Name = "Мини бар",
                    Description = "В мини баре номера хранятся всегда прохладные напитки",
                    Price = 5m
                },
                new Service
                {
                    Name = "Заказ еды в номер",
                    Description = "Закажите доставку еды в номер. Сотрудник отеля принесет в номер заказ в течении 5 минут",
                    Price = 3m
                },
                new Service
                {
                    Name = "Завтрак",
                    Description = "Для постояльцев отеля предусмотрен бесплатный завтрак",
                    Price = 0m
                },
                new Service
                {
                    Name = "Парковка",
                    Description = "Оставьте машину у главного входа и сотрудник отеля припаркует машину в гараже отеля",
                    Price = 5m
                },
                new Service
                {
                    Name = "Wi-Fi",
                    Description = "Для постояльцев отеля предоставляется бесплатный Wi-Fi",
                    Price = 0m
                },
                new Service
                {
                    Name = "Зарядные станции",
                    Description = "Во всех номерах гостиницы установленны зарядные станции для сматрфонов",
                    Price = 0m
                },
                new Service
                {
                    Name = "Аренда конференц-зала",
                    Description = "Аренда конференц-зала для проведения переговоров",
                    Price = 200m
                },
                new Service
                {
                    Name = "Порча имущества отеля",
                    Description = "hidden",
                    Price = 25m
                },
                new Service
                {
                    Name = "Утрата имущества отеля",
                    Description = "hidden",
                    Price = 60m
                }
                );
            await context.SaveChangesAsync();
        }
        private static async Task SeedReservationStatusesAsync(HMSContext context)
        {
            if (context.ReservationStatuses.Any())
            {
                return;
            }
            context.ReservationStatuses.AddRange(
                new ReservationStatus
                {
                    Name = "Performed",
                    Description = "Reservation performed"
                },
                new ReservationStatus
                {
                    Name = "Cancel",
                    Description = "Reservation canceled"
                }
                );
            await context.SaveChangesAsync();
        }
        private static async Task SeedReservationTypesAsync(HMSContext context)
        {
            if (context.ReservationTypes.Any())
            {
                return;
            }
            context.ReservationTypes.AddRange(
                new ReservationType
                {
                    Name = "Web",
                    Description = "Web reservation"
                },
                new ReservationType
                {
                    Name = "Direct",
                    Description = "Reservation direct in hotel"
                }
                );
            await context.SaveChangesAsync();
        }
        private static async Task SeedTransactionStatusesAsync(HMSContext context)
        {
            if (context.TransactionStatuses.Any())
            {
                return;
            }
            context.TransactionStatuses.AddRange(
                new TransactionStatus
                {
                    Name = "Payed",
                    Description = "Payed transaction"
                },
                new TransactionStatus
                {
                    Name = "NotPayed",
                    Description = "Not payed yet transaction"
                }
                );
            await context.SaveChangesAsync();
        }
        private static async Task SeedPaymentTypesAsync(HMSContext context)
        {
            if (context.PaymentTypes.Any())
            {
                return;
            }
            context.PaymentTypes.AddRange(
                new PaymentType
                {
                    Name = "Cash",
                    Description = "Pay in cash"
                },
                new PaymentType
                {
                    Name = "Card",
                    Description = "Pay from card"
                }
                );
            await context.SaveChangesAsync();
        }
        private static async Task SeedGendersAsync(HMSContext context)
        {
            if (context.Genders.Any())
            {
                return;
            }
            context.Genders.AddRange(
                new Gender
                {
                    Name = "Male"
                },
                new Gender
                {
                    Name = "Female"
                }
                );
            await context.SaveChangesAsync();
        }
    }
}
