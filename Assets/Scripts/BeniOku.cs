using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeniOku : MonoBehaviour
{
    /*
     
    => score : Oyuncunun aktif levelde aldigi score , totalScore : Oyuncunun levellerde aldigi skorlarin toplami.
    bu score GameController da tutulurken, totalScore playerprephfs ile tutuluyor. Farkli oyunlarda farkli isler
    icin kullanilabilirler. Ornegin score degil para birikiyorsa para olarak dusunun. Elmas icin farklý degiskenler
    tanimlanacak. 

    => elmas : Oyuncunun aktif levelde aldigi elmaslar, totalElmas : Oyuncunun levellerde aldigi elmaslarin toplami
    Bu ikisini ekliyoruz. Ancak gerekli olan kullanilir gerekli olmayan kullanilmaz. Yine degiskenlerin tutuldugu yerler
    score ile ayni. elmas yerine farkli collectiblelar da kullanildiðinda yine elmas ismiyle tutulacaktir. Leveldeki elmas 
    sayisi icin bir text ui koymadik simdilik. Total elmas icin var. Gerekli gorulurse elmas da eklenir.
     
     
    =>isContinue : bu bool degiskeni ile oyunun aktif olup olmadigi kontrol ediliyor. Oyuncu fail durumunda ya da
    finishe ulastiginda bu degisken false yapilacak. Oyuncu ilerlerken true yapilacak. Baska kontrollerde de kullanýlabilir.

    => instance : bu degisken her classta sadece bir ornek olusturmak icin yani singleton yapisi icin kullaniliyor.
    Bir classtan public bir degiskene veya fonksiyona ulasmak icin class isminden sonra bu yazilip sonra degisken 
    veya fonksiyon ismi yazilacak ORNEK KULLANIM : UIController.instance.SetScore()  gibi...

    => Singleton yapisi sayesinde player scriptine, ui scriptine veya herhangi bir scripte kendinizin ekleyeceginiz 
    public fonksiyonu veya degiskeni diger scriptler icerisinden yukaridaki ornek kullanimda oldugu gibi cagirip calistirabilirsiniz.
     

    => Oyunlarda collectiblelara çarpýnca oluþacak olan score artýþýnýn deðeri inspektörde PlayerController dan ayarlanacak.
     
     
     */
}
