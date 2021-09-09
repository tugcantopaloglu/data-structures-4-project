using System;
using System.IO; //txt okumak için
using System.Collections.Generic;
using System.Linq;

namespace PROJE_TEST
{
    class Program
    {
        static void Main(string[] args)
        {
            //Ayarlamalar
            int successfullyFound = 0;

            //1-A
            Write2DArray(RandomPoint(10, 100, 100)); //random nokta üretme ve üretilen noktaları ekrana yazdırma
            Write2DArray(RandomPoint(100, 100, 100)); //random nokta üretme ve üretilen noktaları ekrana yazdırma
            //1-B
            WriteTable(DistanceMatrix(RandomPoint(10, 100, 100), 10)); //önce random noktalar üretiyor sonra distance matrixi oluşturuyor sonra da yazdırıyor

            //2. kısım
            //2-B
            Console.WriteLine("Varyans değeri girin:");
            double varyans = Convert.ToDouble(Console.ReadLine());
            Console.WriteLine("Çarpıklık değeri girin:");
            double carpiklik = Convert.ToDouble(Console.ReadLine());
            Console.WriteLine("Basıklık değeri girin:");
            double basiklik = Convert.ToDouble(Console.ReadLine());
            Console.WriteLine("Entropi değeri girin:");
            double entropi = Convert.ToDouble(Console.ReadLine());
            Console.WriteLine("K değeri girin:");
            int k = Convert.ToInt32(Console.ReadLine());
            var enteredValues = KNN(varyans, carpiklik, basiklik, entropi, k, ReadFile("data_banknote_authentication.txt")); //dosya okundu kullanıcıdan veriler alındı 
            Console.WriteLine("HESAPLANAN KNN YAZDIRILIYOR:");
            //tablo için şekil düzenleme
            Console.WriteLine("REFERANS PARALAR:");
            WriteTableHeader("");
            Write2DArray(enteredValues.Item1);
            Console.WriteLine("PARANIN TAHMİN EDİLEN TÜRÜ:" + enteredValues.Item2);
            Console.WriteLine();
            //KNN örnek 3 değerine göre hesaplandı

            //2-C
            double[,] testData = ReadFile("test_data.txt");                                                                                       
            for (int i =0; i < testData.GetLength(0); i++) //test txtsindeki her bir elemanı test edip döndürüyor
            {
                var gatheredValues = KNN(testData[i,0], testData[i, 1], testData[i, 2], testData[i, 3],3, ReadFile("data_banknote_authentication.txt"));
                Console.WriteLine("REFERANS PARALAR:");
                WriteTableHeader("");
                Write2DArray(gatheredValues.Item1);
                Console.WriteLine("PARANIN TAHMİN EDİLEN TÜRÜ:" + enteredValues.Item2);
                Console.WriteLine();
                if(testData[i,4] == enteredValues.Item2)
                {
                    successfullyFound++;
                }
            }
            Console.WriteLine("BAŞARI ORANI:");
            Console.WriteLine(successfullyFound / testData.GetLength(0));
            Console.WriteLine("");



            //2-D
            Console.WriteLine("BELLEKTEKİ VERİLER YAZDIRILIYOR:");
            Write2DArray(ReadFile("test_data.txt"));

        }

        static double[,] RandomPoint(int n, int width, int height)
        {
            double[,] matrixArray = new double[n, 2]; //n satır 3 sütunlu, x ve y değerlerini saklamak için array


            for (int i = 0; i < n; i++)
            {
                Random random = new Random();
                double xIndex = random.NextDouble() * (width); //maximum uzunluğa göre rastgele bir x değeri oluşturuyor - double tipinde
                double yIndex = random.NextDouble() * (height); //maximum uzunluğa göre rastgele bir y değeri oluşturuyor - double tipinde
                xIndex = Math.Round(xIndex, 2);
                yIndex = Math.Round(yIndex, 2); //x ve y değerlerini virgülden sonra 2 basamak olacak şekilde yuvarladık
                matrixArray[i, 0] = xIndex;
                matrixArray[i, 1] = yIndex;
            }
            return matrixArray; //x ve y değerlerinin bulunduğu arrayi döndürüyor
        }

        static double RangeBetweenPoints(double pointOneX, double pointOneY, double pointTwoX, double pointTwoY)
        {
            return Math.Round(Math.Sqrt(Math.Pow(pointTwoX - pointOneX, 2) + Math.Pow(pointTwoY - pointOneY, 2)), 2); //2 nokta arasındaki uzaklığı döndüren metod - 
                                                                                                                      //kök içinde (x2 kare eksi x1 kare + y2 kare eksi y1 kare)
                                                                                                                      //virgülden sonra 2 değer olacak şekilde yuvarlama da yapıldı
        }

        static double TwoPointDistance(double pointX, double pointY)
        {
            return Math.Round(Math.Sqrt(Math.Pow(pointX - pointY, 2)));

        }

        static void Write2DArray(double[,] array) //2d array yaz 
        {
            
            for (int i = 0; i < array.GetLength(0); i++)
            {
                for (int j = 0; j < array.GetLength(1); j++)
                {
                    Console.Write(string.Format("{0,-10} ", array[i, j]));
                    
                }
                Console.Write(Environment.NewLine + Environment.NewLine);
            }

        }

        static double[,] DistanceMatrix(double[,] matrixArray, int n)
        {
            double[,] distanceMatrix = new double[n, n]; //distance matrixi oluşturuldu

            for (int line = 0; line < n; line++) //bu iki döngü matrixArray arrayinde satırları teker teker dolaşıp her satır için önce x sonra y değerini döndürüyor
            {
                for (int column = 0; column < n; column++) //burası önce x sonra y veriyor 
                {
                    distanceMatrix[line, column] = RangeBetweenPoints(matrixArray[line, 0], matrixArray[line, 1], matrixArray[column, 0], matrixArray[column, 1]);// distance matrixin line ve column değerlerini 
                                                                                                                                                                  // index olarak alıp matrixArrayde  bu değerleri bularak işleme sokuyor ve atama yapıyor
                }
            }
            return distanceMatrix;
        }

        static void WriteTableHeader(string tahminlenenTür)
        {
            Console.Write(string.Format("{0,-10} ", "Varyans")); Console.Write(string.Format("{0,-10} ", "Çarpılık")); Console.Write(string.Format("{0,-10} ", "Basıklık"));
            Console.Write(string.Format("{0,-10} ", "Entropi")); Console.Write(string.Format("{0,-10} ", "K Değeri")); Console.Write(string.Format("{0,-10} ", tahminlenenTür));
            Console.WriteLine("");
        }

        static void WriteTable(double[,] matrix) //matrix yazdırma
        {
            System.Console.WriteLine("DISTANCE MATRIX:");
            
            for (int i = 0; i < matrix.GetLength(1); i++)
            {
                if (i == 0)
                {
                    Console.Write(string.Format("{0,-10} ", "*"));
                    Console.Write(string.Format("{0,-10} ", i));
                }
                else
                {
                    Console.Write(string.Format("{0,-10} ", i));
                }
                
            }

            Console.Write(Environment.NewLine + Environment.NewLine);

            for (int line = 0; line < matrix.GetLength(0); line++)
            {
                Console.Write("{0,-10} ", line);
                for (int column = 0; column < matrix.GetLength(1); column++)
                {
                   Console.Write(string.Format("{0,-10} ", matrix[line, column]));
                }
                
                Console.Write(Environment.NewLine + Environment.NewLine);
            }

        }
        
        static double[,] ReadFile(string fileName)
        {
            string[] rawLines = File.ReadAllLines(fileName); //txt okuyor //hedef aldığı lokasyon = \PROJE-TEST\PROJE-TEST\bin\Debug\netcoreapp3.1\data_banknote_authentication.txt
            double[,] lastLines = new double[rawLines.GetLength(0),5]; //txtden okuduğumuz verileri işledikten sonra satır ve sütun olarak aktarabileceğimiz bir liste
            int lastLineIndex = 0;
            foreach (string line in rawLines) //ham txt arrayinde her satırı dolaşıyor
            {
                string[] col = line.Split(","); //virgülleri sütunlara bölüyor
                
                for (int i = 0; i < 5; i++) //sütunları işlenmiş listedeki satırlara sütun olarak atıyor
                {
                    col[i] = col[i].Replace('.', ','); //burada columndaki değerler string olduğu için gerekli double a dönüştürme işlemi yapabilmek için c# standartı olan sayıdan sonra "," için nokayı virgül ile değiştiriyoruz
                    lastLines[lastLineIndex,i] = Math.Round(Convert.ToDouble(col[i]),2); //düzenlenmiş stringi hem double yapıp hem de virgülden sonra 2 sayı olacak şekilde yuvarlıyoruz
                }
                lastLineIndex++;
            }
            return lastLines; //işlenmiş arrayi çıktı veriyor
        }

        //2-A
        static (double[,],int) KNN(double varyans, double carpiklik, double basiklik, double entropi, int k, double[,] lines) //bu metod seçilen paraların bilgileri ve test edilen paranın gerçeklik değeri olmak üzere 2 değeri olan bir tuple döndürüyor
        {

            IDictionary<double, double> calculatedList = new Dictionary<double, double>(); ; // list of list şeklinde 2d benzeri list yaptık
            double[,] selectedMoneyInfo = new double[k,5]; //her işlem yapıldıktan sonra return ile döndürelecek array bu array seçilen paraların özelliklerini ve bulduğumuz tahmini içeriyor
            int realMoney = 0;
            int fakeMoney = 0;
            int isMoneyReal = -1; // 0 ise sahte 1 ise gerçek değerine denk olacak ve ileride yapılacak hesaplamalar ile değeri değiştirilecek paranın gerçeklik değeri


            //değerleri hesaplama başlangıcı
            for (int line = 0; line < lines.GetLength(0); line++)
            {
                int loop = 0;
                double rawDistance = 0;
                double moneyType = -1;
                for (int column = 0; column < 4; column++)
                {
                    double result = 0; //sonuç her döngüde sıfırlanıyor ve atanıyor ki sonraki işlemde sıkıntı yaşanmasın
                    if (loop == 0) //gerekli hesapları her döngüde yapıyor
                    {
                        result = TwoPointDistance(Math.Round(varyans, 2), lines[line, column]);
                    }
                    else if (loop == 1)
                    {
                        result = TwoPointDistance(Math.Round(carpiklik, 2), lines[line, column]);
                    }
                    else if (loop == 2)
                    {
                        result = TwoPointDistance(Math.Round(basiklik, 2), lines[line, column]);
                    }
                    else if (loop == 3)
                    {
                        result = TwoPointDistance(Math.Round(entropi, 2), lines[line, column]);
                    }
                    else //para tipini alıyor
                    {
                        moneyType = lines[line, column];
                    }
                    rawDistance += result; // sonuçlar ekleniyor
                    loop++;
                }
                if (calculatedList.ContainsKey(rawDistance)) { }
                else { calculatedList.Add(rawDistance, line); } //sözlüğe hesaplanan değer ve o hesaplanan değerin txt dosyasındaki hangi değerden hesaplandığının indexini atıyor
                
            }
            var list = calculatedList.Keys.ToList(); //burada key'e yani rawDistance'a göre sıralandı
            list.Sort();

            for (int i = 0; i < k; i++) 
            {
                
                int index = Convert.ToInt32(calculatedList.ElementAt(i).Value); //index değerini arrayde kullanabilmek için double tipinden integer tipine dönüştürdük
                if (lines[index, 4] == 1) //lines[calculatedList[index,5] yapısı sözlükten en düşük değeri seçip o değerin txtden fake veya real olması değerini alıyor //gerçek para değeri 1 ise realMoney değişkenini arttırıyor
                {
                    realMoney++;
                }
                else
                {
                    fakeMoney++;
                }
                

            }
            if (realMoney >= fakeMoney)
            {
                isMoneyReal = 1;
            }
            else
            {
                isMoneyReal = 0;
            }

            for (int i = 0; i < k; i++) //k tane paranın özelliklerini listeye atayacağız (ayrıca satır sayısı)
            {
                int index = Convert.ToInt32(calculatedList.ElementAt(i).Value);
                for (int j = 0; j<5; j++) //(sütun sayısı)
                {
                    selectedMoneyInfo[i, j] = lines[index, j];
                }
            }
            return (selectedMoneyInfo,isMoneyReal); //item1= selectedMoneyInfo , item2= isMoneyReal



        }
  
    }
}

