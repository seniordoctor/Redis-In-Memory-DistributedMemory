# UdemyRedisInMemory

Redis Veri Tipleri nelerdir?

	1. Redis String: Sadece string türünde kayıt değil, tek bir key-value datasını kayıt edebiliriz.
		SET name(key) recep(value) --> yeni kayit, eger varolan kayit var ise uzerine yazar ekleme yapmaz.
		GET name | sname | vs...
		GETRANGE <key> <start> <end> --> GETRANGE name 0 2 => "rec"
		INCR <key> --> verdigin key'in value degeri int ise +1 yap.
		INCRBY <key> 10 | ... --> value degeri int ise verilen deger kadar arttir.
		DECR <key> --> azaltmak icin kullanilir.
		DECRBY <key> 10 --> 10 azaltmak icin kullanilir.
		APPEND <key> rcp --> key'e ekleme yapma.

	2. Redis List: Dizi seklinde data tutulabilir. Her yeni eklenen data aksi yazilmadikca sona eklenir.
		LPUSH kitaplar kitap1 --> list'e kitap1, 2... gibi eklemeler yapar. Dizinin basina ekler Left Push oldugu icin
		RPUSH kitaplar kitap3 --> list'e kitap3 ekler, dizinin sonuna ekler Right Push.
		LRANGE kitaplar 0 -1  --> kac adet data varsa alacaktir.
		LRANGE kitaplar 0 2   --> 0, 1, ve 2. indexi alacaktir.
		LPOP kitaplar  		  --> dizini basindan siler. Index 0 silinir, RPOP ile dizinin sonundan sileriz.
		LINDEX kitaplar 1     --> 1. Index'i getirir.

 	3. Redis Set: List veri tipi gibi icerisinde dizin tutabiliriz, 2 onemli farkı var:
 		Dizin icinde tutulacak datalar unique olmali, Random bir sekilde data eklenir basa ve sona eklenmesini biz belirlemiyoruz.
 		SADD renkler mavi --> SET turunde bir key ile ekleme yapariz, value unique olmali
 		SMEMBERS renkler  --> butun value'lari gorebiliriz.

	4. Redis Sorted Set: Siralama uzerine veri tipi. Set'in siralamali versiyonu gibi dusunebilirsin.
		ZADD kalemler <score> kalem1 	--> score kismina verilen degere gore siralanir. kalem1'den 2 tane olamaz unique, yeniden eklersen guncellenir. Score birden fazla ayni olabilir non-unique
		ZRANGE kalemler 0 -1 		    --> butun kalemleri -1 ile goruntulenir.
		ZRANGE kalemler 0 -1 WITHSCORES --> score ile birlikte goruntulenir.
		ZREM kalemler kalem1 			--> kalem1 silinir.

	5. Redis hash: dizin olarak, key-value olarak dictionary class'a benzer. ornekteki sozluk bir <key>
		HMSET sozluk pen kalem 			--> pen : kalem olarak tutulur.
		HMGET sozluk pen 				--> pen'e karsilik olan value degerini getirir.
		HDEL sozluk pen 				--> pen'i siler.
		HGETALL sozluk 					--> sozlukte olan butun hepsinin getirir.
