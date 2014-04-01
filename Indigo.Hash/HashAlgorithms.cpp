#include <stdio.h>
#include <stdlib.h>
#include <string.h>

typedef unsigned long long DIGEST_TYPE;

#undef DIRECT_ROTATE
#define NEED_STRREV


/*

Hash efficiency test battery

(c) 2009 - 2010 Alexander Myasnikov

MaHash algorithms are designed by A.Myasnikov, other hash functions are only implemented by me.

Some code is implemented by Arash Partow and located on partow.net

*/

/* number of words to check */

#define NUM_WORDS 2726299

typedef struct
{
	DIGEST_TYPE digest;
	int used;
	/*
	char text[256 << 1];
	*/

} hash_table;

hash_table *HashTab;
DIGEST_TYPE *SortTab;


enum HashTypes
{
	ht_ROT13Hash, ht_RSHash, ht_PJWHash, ht_MaHash, ht_MaHash1, ht_MaHash3,
	ht_MaHash2, ht_MaHash4, ht_MaHash4v64, ht_MaHash5, ht_MaHash6, ht_MaHash7, ht_MaHash8, ht_MaHash8v64,
	ht_MaHash9, ht_MaHash10,
	ht_MaHash11, ht_JSHash, ht_ELFHash, ht_BKDRHash, ht_maBKDRHash,
	ht_DEKHash, ht_APHash, ht_LYHash, ht_bob_faq6_hash, ht_bob_lookup3_hash,
	ht_crc32, ht_SDBMHash, ht_DJBHash, ht_BPHash, ht_FNVHash, ht_FNV1Hash, ht_FNV1aHash, ht_MurmurHash2, ht_MurmurHash2A, ht_MurmurHash2AM, ht_MurmurHash3,
	ht_maPrime, ht_maPrime2, ht_maPrime2a, ht_maPrime2b, ht_maPrime2c, ht_maPrime2d, ht_SuperFastHash,
	ht_OneAtTimeHash, ht_GoulburnHash, ht_SBOXHash, ht_Crap8Hash, ht_CrapWowHash,
	ht_NovakHash, ht_AdlerHash, ht_FletcherHash, ht_x17Hash, ht_x31Hash, ht_maHashMR1
};

unsigned int avg_results[ht_maHashMR1 + 1];



#ifdef NEED_STRREV
char *strrev(char *str)
{
	unsigned len = strlen(str) - 1; /* Minus one so we don't move the null character */
	unsigned int count;
	char ch;
	for (count = 0; count < len; count++, len--)
	{
		ch = str[count];
		str[count] = str[len];
		str[len] = ch;
	}

	return str;
}
#endif




const unsigned int g_table0[256] =
{
	4143812366UL, 2806512183UL, 4212398656UL, 393834663UL,
	3943187971UL, 847901099UL, 3746904015UL, 2990585247UL,
	4243977488UL, 4075301976UL, 2737181671UL, 2429701352UL,
	4196558752UL, 3152011060UL, 1432515895UL, 204108242UL,
	1180540305UL, 922583281UL, 1734842702UL, 1453807349UL,
	507756934UL, 1553886700UL, 2005976083UL, 3346025117UL,
	97642817UL, 2510760451UL, 4103916440UL, 3222467334UL,
	1312447049UL, 522841194UL, 3955607179UL, 3028936967UL,
	2763655970UL, 3033075496UL, 1935362065UL, 512912210UL,
	2660383701UL, 1652921526UL, 260485165UL, 141882627UL,
	2895806269UL, 804034013UL, 1356707616UL, 3942447612UL,
	2875374199UL, 81028672UL, 1055595160UL, 2755907176UL,
	2880512448UL, 1232977841UL, 3719796487UL, 2940441976UL,
	3739585976UL, 168332576UL, 1318372270UL, 3173546601UL,
	3992298512UL, 3785690335UL, 3667530757UL, 3101895251UL,
	2789438017UL, 3213463724UL, 3067100319UL, 2554433152UL,
	794184286UL, 2599814956UL, 1251486151UL, 4214997752UL,
	690900134UL, 323888098UL, 1537487787UL, 1155362310UL,
	1826165850UL, 2358083425UL, 2957662097UL, 2514517438UL,
	1828367703UL, 3847031274UL, 2308450901UL, 955547506UL,
	1037823031UL, 2922505570UL, 2544914051UL, 2572931499UL,
	442837508UL, 1873354958UL, 2004376537UL, 25413657UL,
	3560636876UL, 1768043132UL, 2870782748UL, 1031556958UL,
	715180405UL, 201079975UL, 4116730284UL, 2748714587UL,
	1091411202UL, 33354499UL, 1931487277UL, 1039106939UL,
	3327011403UL, 396608379UL, 3447523131UL, 301432924UL,
	3180185526UL, 1780290520UL, 3909968679UL, 2398211959UL,
	3704875308UL, 66082280UL, 601805180UL, 3226323057UL,
	3284786200UL, 2282257088UL, 700775591UL, 3528928994UL,
	1601645543UL, 120115228UL, 568698020UL, 178214456UL,
	41846783UL, 897656032UL, 3309570546UL, 2624714322UL,
	2542948622UL, 1168171675UL, 2460933760UL, 93808223UL,
	2384991231UL, 4268721795UL, 4001720080UL, 1516739672UL,
	4111847489UL, 810915309UL, 1238071781UL, 935043360UL,
	2020231594UL, 37717498UL, 3603218947UL, 1534593867UL,
	2819275526UL, 1965883441UL, 674162751UL, 128087286UL,
	4138356188UL, 543626850UL, 1355906380UL, 3565721429UL,
	1142978716UL, 1614752605UL, 1624389156UL, 3363454971UL,
	2029311310UL, 2249603714UL, 3448236784UL, 1764058505UL,
	2198836711UL, 3481576182UL, 3168665556UL, 3834682664UL,
	1979945243UL, 3456525349UL, 2721891322UL, 1099639387UL,
	1528675965UL, 3069012165UL, 1807951214UL, 1901014398UL,
	2805656341UL, 3321210152UL, 2317543573UL, 1015607418UL,
	178584554UL, 4020226276UL, 492648819UL, 97778844UL,
	4134244261UL, 1389599433UL, 331211243UL, 3769684011UL,
	2036127367UL, 3174548433UL, 3241354897UL, 2570869934UL,
	3071842004UL, 1972073698UL, 48467379UL, 1015444026UL,
	3126762609UL, 1104264591UL, 3096375666UL, 1380392409UL,
	684368280UL, 1493310388UL, 2109527660UL, 3034364089UL,
	3168522906UL, 3042350939UL, 3696929834UL, 3410250713UL,
	3726870750UL, 3357455860UL, 1816295563UL, 2678332086UL,
	26178399UL, 614899533UL, 2248041911UL, 1431155883UL,
	1184971826UL, 3711847923UL, 2744489682UL, 168580352UL,
	694400736UL, 2659092308UL, 811197288UL, 1093111228UL,
	824677015UL, 2041709752UL, 1650020171UL, 2344240270UL,
	3773698958UL, 3393428365UL, 3498636527UL, 556541408UL,
	1883820721UL, 3249806350UL, 3635420446UL, 1661145756UL,
	3087642385UL, 1620143845UL, 3852949019UL, 1054565053UL,
	3574021829UL, 2466085457UL, 2078148836UL, 460565767UL,
	4097474724UL, 1381665351UL, 1652238922UL, 2200252397UL,
	3726797486UL, 4001080204UL, 259576503UL, 567653141UL,
	325219513UL, 1227314237UL, 3191441965UL, 1433728871UL,
	4198425173UL, 2908977223UL, 3757065246UL, 294312130UL,
	4136006097UL, 3409363054UL, 2112383431UL, 1177366649UL
};



const unsigned int g_table1[128] =
{
	826524031UL, 360568984UL, 3001046685UL, 1511935255UL,
	1287825396UL, 3167385669UL, 1488463483UL, 4077470910UL,
	1360843071UL, 986771770UL, 2307292828UL, 3845679814UL,
	1429883439UL, 1990257475UL, 4087625806UL, 1700033651UL,
	1388994450UL, 935547107UL, 3237786789UL, 644530675UL,
	2274037095UL, 888755779UL, 3020158166UL, 2136355264UL,
	2558959443UL, 1751931693UL, 2325730565UL, 3029134627UL,
	668542860UL, 2140243729UL, 2384660990UL, 666440934UL,
	842610975UL, 1563602260UL, 1429103271UL, 899918690UL,
	3441536151UL, 4078621296UL, 1527765522UL, 4191433361UL,
	222526771UL, 309447417UL, 2035245353UL, 3730203536UL,
	3330019758UL, 876252573UL, 2545027471UL, 453932528UL,
	282738293UL, 1826993794UL, 1569532013UL, 543681326UL,
	3097574376UL, 2336551794UL, 1563241416UL, 1127019882UL,
	3088670038UL, 2766122176UL, 3706267663UL, 1110947226UL,
	2608363541UL, 3166834418UL, 1310161541UL, 755904436UL,
	2922000163UL, 3815555181UL, 1578365408UL, 3137960721UL,
	3254556244UL, 4287631844UL, 750375141UL, 1481489491UL,
	1903967768UL, 3684774106UL, 765971482UL, 3225162750UL,
	2946561128UL, 1920278401UL, 1803486497UL, 4166913456UL,
	1855615192UL, 1934651772UL, 1736560291UL, 2101779280UL,
	3560837687UL, 3004438879UL, 804667617UL, 2969326308UL,
	3118017313UL, 3090405800UL, 566615197UL, 2451279063UL,
	4029572038UL, 2612593078UL, 3831703462UL, 914594646UL,
	2873305199UL, 2860901605UL, 3296630085UL, 1273702937UL,
	2852911938UL, 1003268745UL, 1387783190UL, 159227777UL,
	2211994285UL, 28095103UL, 3659848176UL, 3976935977UL,
	3301276082UL, 2641346573UL, 651238838UL, 2264520966UL,
	1484747269UL, 3016251036UL, 3857206301UL, 91952846UL,
	1662449304UL, 2028491746UL, 1613452911UL, 2409055848UL,
	1453868667UL, 4146146473UL, 1646176015UL, 3769580099UL,
	3171524988UL, 2980516679UL, 828895558UL, 3384493282UL
};




DIGEST_TYPE __declspec(dllexport) GoulburnHash(char *cp, unsigned int len)

{
	register unsigned int h = 0, u;


	for (u = 0; u<len; ++u)
	{
		h += g_table0[(unsigned char)cp[u]];
		h ^= (h << 3) ^ (h >> 29);
		h += g_table1[h >> 25];
		h ^= (h << 14) ^ (h >> 18);
		h += 1783936964UL;
	}
	return h;
}


DIGEST_TYPE __declspec(dllexport) OneAtTimeHash(char *key, unsigned int key_len)
{
	unsigned int hash = 0;
	size_t i;

	for (i = 0; i < key_len; i++)
	{
		hash += (unsigned char)key[i];
		hash += (hash << 10);
		hash ^= (hash >> 6);
	}
	hash += (hash << 3);
	hash ^= (hash >> 11);
	hash += (hash << 15);
	return hash;
}




const unsigned int SBOX_table[256] =
{
	0x4660c395, 0x3baba6c5, 0x27ec605b, 0xdfc1d81a, 0xaaac4406, 0x3783e9b8, 0xa4e87c68, 0x62dc1b2a,
	0xa8830d34, 0x10a56307, 0x4ba469e3, 0x54836450, 0x1b0223d4, 0x23312e32, 0xc04e13fe, 0x3b3d61fa,
	0xdab2d0ea, 0x297286b1, 0x73dbf93f, 0x6bb1158b, 0x46867fe2, 0xb7fb5313, 0x3146f063, 0x4fd4c7cb,
	0xa59780fa, 0x9fa38c24, 0x38c63986, 0xa0bac49f, 0xd47d3386, 0x49f44707, 0xa28dea30, 0xd0f30e6d,
	0xd5ca7704, 0x934698e3, 0x1a1ddd6d, 0xfa026c39, 0xd72f0fe6, 0x4d52eb70, 0xe99126df, 0xdfdaed86,
	0x4f649da8, 0x427212bb, 0xc728b983, 0x7ca5d563, 0x5e6164e5, 0xe41d3a24, 0x10018a23, 0x5a12e111,
	0x999ebc05, 0xf1383400, 0x50b92a7c, 0xa37f7577, 0x2c126291, 0x9daf79b2, 0xdea086b1, 0x85b1f03d,
	0x598ce687, 0xf3f5f6b9, 0xe55c5c74, 0x791733af, 0x39954ea8, 0xafcff761, 0x5fea64f1, 0x216d43b4,
	0xd039f8c1, 0xa6cf1125, 0xc14b7939, 0xb6ac7001, 0x138a2eff, 0x2f7875d6, 0xfe298e40, 0x4a3fad3b,
	0x066207fd, 0x8d4dd630, 0x96998973, 0xe656ac56, 0xbb2df109, 0x0ee1ec32, 0x03673d6c, 0xd20fb97d,
	0x2c09423c, 0x093eb555, 0xab77c1e2, 0x64607bf2, 0x945204bd, 0xe8819613, 0xb59de0e3, 0x5df7fc9a,
	0x82542258, 0xfb0ee357, 0xda2a4356, 0x5c97ab61, 0x8076e10d, 0x48e4b3cc, 0x7c28ec12, 0xb17986e1,
	0x01735836, 0x1b826322, 0x6602a990, 0x7c1cef68, 0xe102458e, 0xa5564a67, 0x1136b393, 0x98dc0ea1,
	0x3b6f59e5, 0x9efe981d, 0x35fafbe0, 0xc9949ec2, 0x62c765f9, 0x510cab26, 0xbe071300, 0x7ee1d449,
	0xcc71beef, 0xfbb4284e, 0xbfc02ce7, 0xdf734c93, 0x2f8cebcd, 0xfeedc6ab, 0x5476ee54, 0xbd2b5ff9,
	0xf4fd0352, 0x67f9d6ea, 0x7b70db05, 0x5a5f5310, 0x482dd7aa, 0xa0a66735, 0x321ae71f, 0x8e8ad56c,
	0x27a509c3, 0x1690b261, 0x4494b132, 0xc43a42a7, 0x3f60a7a6, 0xd63779ff, 0xe69c1659, 0xd15972c8,
	0x5f6cdb0c, 0xb9415af2, 0x1261ad8d, 0xb70a6135, 0x52ceda5e, 0xd4591dc3, 0x442b793c, 0xe50e2dee,
	0x6f90fc79, 0xd9ecc8f9, 0x063dd233, 0x6cf2e985, 0xe62cfbe9, 0x3466e821, 0x2c8377a2, 0x00b9f14e,
	0x237c4751, 0x40d4a33b, 0x919df7e8, 0xa16991a4, 0xc5295033, 0x5c507944, 0x89510e2b, 0xb5f7d902,
	0xd2d439a6, 0xc23e5216, 0xd52d9de3, 0x534a5e05, 0x762e73d4, 0x3c147760, 0x2d189706, 0x20aa0564,
	0xb07bbc3b, 0x8183e2de, 0xebc28889, 0xf839ed29, 0x532278f7, 0x41f8b31b, 0x762e89c1, 0xa1e71830,
	0xac049bfc, 0x9b7f839c, 0x8fd9208d, 0x2d2402ed, 0xf1f06670, 0x2711d695, 0x5b9e8fe4, 0xdc935762,
	0xa56b794f, 0xd8666b88, 0x6872c274, 0xbc603be2, 0x2196689b, 0x5b2b5f7a, 0x00c77076, 0x16bfa292,
	0xc2f86524, 0xdd92e83e, 0xab60a3d4, 0x92daf8bd, 0x1fe14c62, 0xf0ff82cc, 0xc0ed8d0a, 0x64356e4d,
	0x7e996b28, 0x81aad3e8, 0x05a22d56, 0xc4b25d4f, 0x5e3683e5, 0x811c2881, 0x124b1041, 0xdb1b4f02,
	0x5a72b5cc, 0x07f8d94e, 0xe5740463, 0x498632ad, 0x7357ffb1, 0x0dddd380, 0x3d095486, 0x2569b0a9,
	0xd6e054ae, 0x14a47e22, 0x73ec8dcc, 0x004968cf, 0xe0c3a853, 0xc9b50a03, 0xe1b0eb17, 0x57c6f281,
	0xc9f9377d, 0x43e03612, 0x9a0c4554, 0xbb2d83ff, 0xa818ffee, 0xf407db87, 0x175e3847, 0x5597168f,
	0xd3d547a7, 0x78f3157c, 0xfc750f20, 0x9880a1c6, 0x1af41571, 0x95d01dfc, 0xa3968d62, 0xeae03cf8,
	0x02ee4662, 0x5f1943ff, 0x252d9d1c, 0x6b718887, 0xe052f724, 0x4cefa30b, 0xdcc31a00, 0xe4d0024d,
	0xdbb4534a, 0xce01f5c8, 0x0c072b61, 0x5d59736a, 0x60291da4, 0x1fbe2c71, 0x2f11d09c, 0x9dce266a,
};




DIGEST_TYPE __declspec(dllexport) SBOXHash(char * data, unsigned int len)
{

	unsigned int h = len;
	unsigned char * key = (unsigned char *)data;

	for (; len &~1; len -= 2, key += 2)
		h = (((h ^ SBOX_table[key[0]]) * 3) ^ SBOX_table[key[1]]) * 3;
	if (len & 1)
		h = (h ^ SBOX_table[key[0]]) * 3;
	h += (h >> 22) ^ (h << 4);
	return h;
}


DIGEST_TYPE __declspec(dllexport) Crap8Hash(char * key, unsigned int len)
{


#define c8fold( a, b, lo, hi ) { p = (unsigned int)(a) * (unsigned long long)(b); lo ^= (unsigned int)p; hi ^= (unsigned int)(p >> 32); }
#define c8mix( in ) { h *= m; c8fold( in, m, k, h ); }

	const unsigned int m = 0x83d2e73b, n = 0x97e1cc59, *key4 = (const unsigned int *)key;
	unsigned int h = len, k = n + len;
	unsigned long long p;

	while (len >= 8)
	{
		c8mix(key4[0]) c8mix(key4[1]) key4 += 2;
		len -= 8;
	}
	if (len >= 4)
	{
		c8mix(key4[0]) key4 += 1;
		len -= 4;
	}
	if (len)
	{
		c8mix(key4[0] & ((1 << (len * 8)) - 1))
	}
	c8fold(h ^ k, n, k, k)
		return k;
}


DIGEST_TYPE __declspec(dllexport) CrapWowHash(char * key, unsigned int len)
{


#define cwfold( a, b, lo, hi ) { p = (unsigned int)(a) * (unsigned long long)(b); lo ^= (unsigned int)p; hi ^= (unsigned int)(p >> 32); }
#define cwmixa( in ) { cwfold( in, m, k, h ); }
#define cwmixb( in ) { cwfold( in, n, h, k ); }

	const unsigned int m = 0x57559429, n = 0x5052acdb, *key4 = (const unsigned int *)key;
	unsigned int h = len, k = len + n;
	unsigned long long p;

	while (len >= 8)
	{
		cwmixb(key4[0]) cwmixa(key4[1]) key4 += 2;
		len -= 8;
	}
	if (len >= 4)
	{
		cwmixb(key4[0]) key4 += 1;
		len -= 4;
	}
	if (len)
	{
		cwmixa(key4[0] & ((1 << (len * 8)) - 1))
	}
	cwmixb(h ^ (k + n))
		return k ^ h;


}




#define get16bits(d) (*((const unsigned short *) (d)))


DIGEST_TYPE __declspec(dllexport) SuperFastHash(char * data, unsigned int len)
{
	unsigned int hash = len, tmp;
	int rem;


	if (len <= 0 || data == NULL) return 0;

	rem = len & 3;
	len >>= 2;

	/* Main loop */
	for (; len > 0; len--)
	{
		hash += get16bits(data);
		tmp = (get16bits(data + 2) << 11) ^ hash;
		hash = (hash << 16) ^ tmp;
		data += 4;
		hash += hash >> 11;
	}

	/* Handle end cases */
	switch (rem)
	{
	case 3:
		hash += get16bits(data);
		hash ^= hash << 16;
		hash ^= data[2] << 18;
		hash += hash >> 11;
		break;
	case 2:
		hash += get16bits(data);
		hash ^= hash << 11;
		hash += hash >> 17;
		break;
	case 1:
		hash += *data;
		hash ^= hash << 10;
		hash += hash >> 1;
	}

	/* Force "avalanching" of final 127 bits */
	hash ^= hash << 3;
	hash += hash >> 5;
	hash ^= hash << 4;
	hash += hash >> 17;
	hash ^= hash << 25;
	hash += hash >> 6;

	return hash;
}



DIGEST_TYPE __declspec(dllexport) MurmurHash2(char * key, unsigned int len)
{
	const unsigned int m = 0x5bd1e995;
	const unsigned int seed = 0;
	const int r = 24;

	unsigned int h = seed ^ len;

	const unsigned char * data = (const unsigned char *)key;

	while (len >= 4)
	{
		unsigned int k;

		k = data[0];
		k |= data[1] << 8;
		k |= data[2] << 16;
		k |= data[3] << 24;

		k *= m;
		k ^= k >> r;
		k *= m;

		h *= m;
		h ^= k;

		data += 4;
		len -= 4;
	}

	switch (len)
	{
	case 3:
		h ^= data[2] << 16;
	case 2:
		h ^= data[1] << 8;
	case 1:
		h ^= data[0];
		h *= m;
	};

	h ^= h >> 13;
	h *= m;
	h ^= h >> 15;

	return h;
}



#define mmix(h,k) { k *= m; k ^= k >> r; k *= m; h *= m; h ^= k; }

DIGEST_TYPE __declspec(dllexport) MurmurHash2A(char * key, unsigned int len)
{
	const unsigned int m = 0x5bd1e995;
	const int r = 24;
	unsigned int l = len, h = 0;

	const unsigned char * data = (const unsigned char *)key;


	while (len >= 4)
	{
		unsigned int k = *(unsigned int*)data;

		mmix(h, k);

		data += 4;
		len -= 4;
	}

	unsigned int t = 0;

	switch (len)
	{
	case 3:
		t ^= data[2] << 16;
	case 2:
		t ^= data[1] << 8;
	case 1:
		t ^= data[0];
	};

	mmix(h, t);
	mmix(h, l);

	h ^= h >> 13;
	h *= m;
	h ^= h >> 15;

	return h;

}






DIGEST_TYPE __declspec(dllexport) MurmurHash2AM(char * key, unsigned int len)
{
	const unsigned int m = 0x5bd1e995;
	const int r = 24;
	unsigned int l = len, h = 0, i = 1;

	const unsigned char * data = (const unsigned char *)key;


	while (len >= 4)
	{
		unsigned int k = *(unsigned int*)data + i++;

		mmix(h, k);

		data += 4;
		len -= 4;
	}

	unsigned int t = 0;

	switch (len)
	{
	case 3:
		t ^= data[2] << 16;
	case 2:
		t ^= data[1] << 8;
	case 1:
		t ^= data[0];
	};

	mmix(h, t);
	mmix(h, l);

	h ^= h >> 13;
	h *= m;
	h ^= h >> 15;

	return h;

}




#define mhrot(x,k) (((x)<<(k)) | ((x)>>(32-(k))))


DIGEST_TYPE __declspec(dllexport) MurmurHash3(char * key, unsigned int len)
{
#define mmix3(h,k) { k *= m1; k = mhrot(k,r1); k *= m2; h *= 3; h ^= k; }

	const unsigned int m1 = 0x0acffe3d, m2 = 0x0e4ef5f3, m3 = 0xa729a897;
	const unsigned int r1 = 11, r2 = 18, r3 = 18;
	unsigned char *tail;

	unsigned int h = len, k = 0;

	unsigned int *dwords = (unsigned int *)key;

	while (len >= 4)
	{
		k = *dwords++;
		mmix3(h, k);
		len -= 4;
	}


	tail = (unsigned char *)dwords;

	switch (len)
	{
	case 3:
		k ^= tail[2] << 16;
	case 2:
		k ^= tail[1] << 8;
	case 1:
		k ^= tail[0];
		mmix3(h, k);
	};

	h ^= h >> r2;
	h *= m3;
	h ^= h >> r3;
	return h;
};


DIGEST_TYPE __declspec(dllexport)
ROT13Hash(char *str, unsigned int len)
{
	unsigned int hash = 0;
	unsigned int i = 0;

	for (i = 0; i < len; str++, i++)
	{
		hash += (unsigned char)(*str);
		hash -= (hash << 13) | (hash >> 19);
	}

	return hash;
}


DIGEST_TYPE __declspec(dllexport)
RSHash(char *str, unsigned int len)
{
	unsigned int b = 378551;
	unsigned int a = 63689;
	unsigned int hash = 0;
	unsigned int i = 0;

	for (i = 0; i < len; str++, i++)
	{
		hash = hash * a + (unsigned char)(*str);
		a = a * b;
	}

	return hash;
}


DIGEST_TYPE __declspec(dllexport)
PJWHash(char *str, unsigned int len)
{
	unsigned int BitsInUnsignedInt = (unsigned int)(sizeof (unsigned int)* 8);
	unsigned int ThreeQuarters = (unsigned int)((BitsInUnsignedInt * 3) / 4);
	unsigned int OneEighth = (unsigned int)(BitsInUnsignedInt / 8);
	unsigned int HighBits = (unsigned int)(0xFFFFFFFF) <<
		(BitsInUnsignedInt - OneEighth);
	unsigned int hash = 0;
	unsigned int test = 0;
	unsigned int i = 0;

	for (i = 0; i < len; str++, i++)
	{
		hash = (hash << OneEighth) + (unsigned char)(*str);

		if ((test = hash & HighBits) != 0)
		{
			hash = ((hash ^ (test >> ThreeQuarters)) & (~HighBits));
		}
	}
	return hash;
}




static const unsigned char sTable[256] =
{
	0xa3, 0xd7, 0x09, 0x83, 0xf8, 0x48, 0xf6, 0xf4, 0xb3, 0x21, 0x15, 0x78,
	0x99, 0xb1, 0xaf, 0xf9, 0xe7, 0x2d, 0x4d, 0x8a, 0xce, 0x4c, 0xca, 0x2e,
	0x52, 0x95, 0xd9, 0x1e, 0x4e, 0x38, 0x44, 0x28, 0x0a, 0xdf, 0x02, 0xa0,
	0x17, 0xf1, 0x60, 0x68, 0x12, 0xb7, 0x7a, 0xc3, 0xe9, 0xfa, 0x3d, 0x53,
	0x96, 0x84, 0x6b, 0xba, 0xf2, 0x63, 0x9a, 0x19, 0x7c, 0xae, 0xe5, 0xf5,
	0xf7, 0x16, 0x6a, 0xa2, 0x39, 0xb6, 0x7b, 0x0f, 0xc1, 0x93, 0x81, 0x1b,
	0xee, 0xb4, 0x1a, 0xea, 0xd0, 0x91, 0x2f, 0xb8, 0x55, 0xb9, 0xda, 0x85,
	0x3f, 0x41, 0xbf, 0xe0, 0x5a, 0x58, 0x80, 0x5f, 0x66, 0x0b, 0xd8, 0x90,
	0x35, 0xd5, 0xc0, 0xa7, 0x33, 0x06, 0x65, 0x69, 0x45, 0x00, 0x94, 0x56,
	0x6d, 0x98, 0x9b, 0x76, 0x97, 0xfc, 0xb2, 0xc2, 0xb0, 0xfe, 0xdb, 0x20,
	0xe1, 0xeb, 0xd6, 0xe4, 0xdd, 0x47, 0x4a, 0x1d, 0x42, 0xed, 0x9e, 0x6e,
	0x49, 0x3c, 0xcd, 0x43, 0x27, 0xd2, 0x07, 0xd4, 0xde, 0xc7, 0x67, 0x18,
	0x89, 0xcb, 0x30, 0x1f, 0x8d, 0xc6, 0x8f, 0xaa, 0xc8, 0x74, 0xdc, 0xc9,
	0x5d, 0x5c, 0x31, 0xa4, 0x70, 0x88, 0x61, 0x2c, 0x9f, 0x0d, 0x2b, 0x87,
	0x50, 0x82, 0x54, 0x64, 0x26, 0x7d, 0x03, 0x40, 0x34, 0x4b, 0x1c, 0x73,
	0xd1, 0xc4, 0xfd, 0x3b, 0xcc, 0xfb, 0x7f, 0xab, 0xe6, 0x3e, 0x5b, 0xa5,
	0xad, 0x04, 0x23, 0x9c, 0x14, 0x51, 0x22, 0xf0, 0x29, 0x79, 0x71, 0x7e,
	0xff, 0x8c, 0x0e, 0xe2, 0x0c, 0xef, 0xbc, 0x72, 0x75, 0x6f, 0x37, 0xa1,
	0xec, 0xd3, 0x8e, 0x62, 0x8b, 0x86, 0x10, 0xe8, 0x08, 0x77, 0x11, 0xbe,
	0x92, 0x4f, 0x24, 0xc5, 0x32, 0x36, 0x9d, 0xcf, 0xf3, 0xa6, 0xbb, 0xac,
	0x5e, 0x6c, 0xa9, 0x13, 0x57, 0x25, 0xb5, 0xe3, 0xbd, 0xa8, 0x3a, 0x01,
	0x05, 0x59, 0x2a, 0x46
};

#ifdef DIRECT_ROTATE
#define LROT(x) _lrotl(x, 11)
#else
#define LROT(x) (((x) << 11) | ((x) >> 21 ))
#endif


#define ITERATIONS 1

DIGEST_TYPE __declspec(dllexport)
MaHash(char *str, unsigned int len)
{
	unsigned int hash = len, i;
	unsigned char byte;

	for (i = 0; i != len * ITERATIONS; i++)
	{

		byte = (unsigned char)*(str + (i % len)) + i;

		hash =
			LROT(hash + ((hash << 6) ^ (hash >> 8))) +
			sTable[byte];

	}

	return hash;
}

#ifdef DIRECT_ROTATE
#define RROT(x) _lrotr(x, 11)
#else
#define RROT(x) (((x) << 21) | ((x) >> 11 ))
#endif

DIGEST_TYPE __declspec(dllexport)
MaHash1(char *str, unsigned int len)
{
	unsigned int t, hash1 = len, hash2 = len, i;
	unsigned char index;

	for (i = 0; i != len; i++, str++)
	{

		index = (unsigned char)(*str) + i;

		hash1 = LROT(hash1 + ((hash1 << 6) ^ (hash1 >> 8)));
		hash1 += sTable[index];
		hash2 = RROT(hash2 + ((hash2 << 6) ^ (hash2 >> 8)));
		hash2 += sTable[index];

		t = hash1;
		hash1 = hash2;
		hash2 = t;

	}


	hash1 = LROT(hash1 + ((hash1 << 6) ^ (hash1 >> 8)));
	hash1 += sTable[len & 255];
	hash2 = RROT(hash2 + ((hash2 << 6) ^ (hash2 >> 8)));
	hash2 += sTable[len & 255];


	return hash1 ^ hash2;
}



DIGEST_TYPE __declspec(dllexport)
MaHash3(char *str, unsigned int len)
{
	unsigned int t, hash1 = len, hash2 = len, i;
	unsigned char index;

	for (i = 0; i != len; i++, str++)
	{

		index = (unsigned char)(*str) + i;

		hash1 += sTable[index];
		hash1 = LROT(hash1 + ((hash1 << 6) ^ (hash1 >> 8)));
		hash2 += sTable[(sTable[index] + 1) & 255];
		hash2 = RROT(hash2 + ((hash2 << 6) ^ (hash2 >> 8)));
		t = hash1;
		hash1 = hash2;
		hash2 = t;

	}

	hash1 = LROT(hash1 + ((hash1 << 6) ^ (hash1 >> 8)));
	hash1 += sTable[len & 255];
	hash2 = RROT(hash2 + ((hash2 << 6) ^ (hash2 >> 8)));
	hash2 += sTable[len & 255];


	return hash1 ^ hash2;
}





DIGEST_TYPE __declspec(dllexport)
MaHash2(char *str, unsigned int len)
{
	unsigned int t, hash1 = len, hash2 = len, i;
	unsigned char index;

	for (i = 0; i != len; i++, str++)
	{

		index = (unsigned char)(*str) + i;

		hash1 += sTable[index];
		hash1 = LROT(hash1 + ((hash1 << 6) ^ (hash1 >> 8)));
		hash2 += sTable[index];
		hash2 = RROT(hash2 + ((hash2 << 6) ^ (hash2 >> 8)));

		t = hash1;
		hash1 = hash2;
		hash2 = t;

	}



	return hash1 ^ hash2;
}




#ifdef DIRECT_ROTATE
#define LROT12(x) _lrotl(x, 12)
#define RROT13(x) _lrotr(x, 13)
#else
#define LROT12(x) (((x) << 12) | ((x) >> 20 ))
#define RROT13(x) (((x) << 19) | ((x) >> 13 ))
#endif


DIGEST_TYPE __declspec(dllexport)
MaHash11(char *str, unsigned int len)
{
	unsigned int t, hash1 = len, hash2 = len, i;
	unsigned char index;

	for (i = 0; i != len; i++, str++)
	{

		index = (unsigned char)(*str) + i;

		hash1 += sTable[index];
		hash1 = LROT12(hash1 + ((hash1 << 6) ^ (hash1 >> 14)));
		hash2 += sTable[index];
		hash2 = RROT13(hash2 + ((hash2 << 6) ^ (hash2 >> 14)));

		t = hash1;
		hash1 = hash2;
		hash2 = t;

	}



	return hash1 ^ hash2;
}


DIGEST_TYPE __declspec(dllexport)
MaHash4(char *str, unsigned int len)
{
	unsigned int sh1, sh2, hash1 = len, hash2 = len, i;
	unsigned char index;

	for (i = 0; i != len; i++, str++)
	{

		index = (unsigned char)(*str) + i;

		hash1 += sTable[index];
		hash1 = LROT(hash1 + ((hash1 << 6) ^ (hash1 >> 8)));
		hash2 += sTable[index];
		hash2 = RROT(hash2 + ((hash2 << 6) ^ (hash2 >> 8)));

		sh1 = hash1;
		sh2 = hash2;

		hash1 = ((sh1 >> 16) & 0xffff) | ((sh2 & 0xffff) << 16);
		hash2 = ((sh2 >> 16) & 0xffff) | ((sh1 & 0xffff) << 16);

	}



	return hash1 ^ hash2;
}


#define HIAVAL

#ifdef HIAVAL
#define LROT64(x) (((x) << 29) | ((x) >> 34 ))
#endif

DIGEST_TYPE __declspec(dllexport)
MaHash4v64(char *str, unsigned int len)
{
	unsigned int sh1, sh2, hash1 = len, hash2 = len, i;
	unsigned char index;
	DIGEST_TYPE digest;

	for (i = 0; i != len; i++, str++)
	{

		index = (unsigned char)(*str) + i;

		hash1 += sTable[index];
		hash1 = LROT(hash1 + ((hash1 << 6) ^ (hash1 >> 8)));
		hash2 += sTable[index];
		hash2 = RROT(hash2 + ((hash2 << 6) ^ (hash2 >> 8)));

		sh1 = hash1;
		sh2 = hash2;

		hash1 = ((sh1 >> 16) & 0xffff) | ((sh2 & 0xffff) << 16);
		hash2 = ((sh2 >> 16) & 0xffff) | ((sh1 & 0xffff) << 16);

	}


#ifdef HIAVAL
	digest = (((DIGEST_TYPE)hash1) << 32) | ((DIGEST_TYPE)hash2);
	digest ^= LROT64(digest + ((digest << 13) ^ (digest >> 23)));
#else
	digest = (((DIGEST_TYPE)hash2) << 32) | ((DIGEST_TYPE)hash1);
#endif

	return digest;

}

#ifdef DIRECT_ROTATE
#define LROT14(x) _lrotl(x, 14)
#define RROT14(x) _lrotr(x, 14)
#else
#define LROT14(x) (((x) << 14) | ((x) >> 18 ))
#define RROT14(x) (((x) << 18) | ((x) >> 14 ))
#endif

DIGEST_TYPE __declspec(dllexport)
MaHash8(char *str, unsigned int len)
{
	unsigned int sh1, sh2, hash1 = len, hash2 = len, i;
	unsigned char index;

	for (i = 0; i != len; i++, str++)
	{

		index = (unsigned char)(*str) + i;

		hash1 += sTable[index];
		hash1 = LROT14(hash1 + ((hash1 << 6) ^ (hash1 >> 11)));
		hash2 += sTable[index];
		hash2 = RROT14(hash2 + ((hash2 << 6) ^ (hash2 >> 11)));

		sh1 = hash1;
		sh2 = hash2;

		hash1 = ((sh1 >> 16) & 0xffff) | ((sh2 & 0xffff) << 16);
		hash2 = ((sh2 >> 16) & 0xffff) | ((sh1 & 0xffff) << 16);

	}



	return hash1 ^ hash2;
}

#define HIAVAL

#ifdef HIAVAL
#define LROT64(x) (((x) << 29) | ((x) >> 34 ))
#endif

DIGEST_TYPE __declspec(dllexport)
MaHash8v64(char *str, unsigned int len)
{
	unsigned int sh1, sh2, hash1 = len, hash2 = len, i;
	unsigned char index;
	DIGEST_TYPE digest;

	for (i = 0; i != len; i++, str++)
	{

		index = (unsigned char)(*str) + i;

		hash1 += sTable[index];
		hash1 = LROT14(hash1 + ((hash1 << 6) ^ (hash1 >> 11)));
		hash2 += sTable[index];
		hash2 = RROT14(hash2 + ((hash2 << 6) ^ (hash2 >> 11)));

		sh1 = hash1;
		sh2 = hash2;

		hash1 = ((sh1 >> 16) & 0xffff) | ((sh2 & 0xffff) << 16);
		hash2 = ((sh2 >> 16) & 0xffff) | ((sh1 & 0xffff) << 16);

	}




#ifdef HIAVAL
	digest = (((DIGEST_TYPE)hash1) << 32) | ((DIGEST_TYPE)hash2);
	digest ^= LROT64(digest + ((digest << 13) ^ (digest >> 23)));
#else
	digest = (((DIGEST_TYPE)hash2) << 32) | ((DIGEST_TYPE)hash1);
#endif

	return digest;
}

#ifdef DIRECT_ROTATE
#define LROT14(x) _lrotl(x, 14)
#define RROT15(x) _lrotr(x, 15)
#else
#define LROT14(x) (((x) << 14) | ((x) >> 18 ))
#define RROT15(x) (((x) << 17) | ((x) >> 15 ))
#endif



DIGEST_TYPE __declspec(dllexport)
MaHash10(char *str, unsigned int len)
{
	unsigned int hash1 = len, hash2 = len, i;
	unsigned char index;


	for (i = 0; i != len; i++, str++)
	{
		index = (unsigned char)(*str) + i;

		hash1 = LROT14(hash2 + ((hash2 << 6) ^ (hash2 >> 11)));
		hash1 += sTable[index];
		hash2 = RROT15(hash1 + ((hash1 << 6) ^ (hash1 >> 11)));
		hash2 += sTable[index];
	}

	return hash1 ^ hash2;
}



DIGEST_TYPE __declspec(dllexport)
MaHash5(char *str, unsigned int len)
{
	unsigned int sh1, sh2, hash1 = len, hash2 = len, i;
	unsigned char index;

	for (i = 0; i != len; i++, str++)
	{

		index = (unsigned char)(*str) + i;

		hash1 += sTable[index];
		hash1 = LROT(hash1 + ((hash1 << 6) ^ (hash1 >> 8)));
		hash2 += sTable[index];
		hash2 = RROT(hash2 + ((hash2 << 6) ^ (hash2 >> 8)));

		sh1 = LROT(hash1 + ((hash1 << 6) ^ (hash1 >> 8)));
		sh2 = RROT(hash2 + ((hash2 << 6) ^ (hash2 >> 8)));

		hash1 = ((sh1 >> 16) & 0xffff) | ((sh2 & 0xffff) << 16);
		hash2 = ((sh2 >> 16) & 0xffff) | ((sh1 & 0xffff) << 16);

	}



	return hash1 ^ hash2;
}


DIGEST_TYPE __declspec(dllexport)
MaHash6(char *str, unsigned int len)
{
	unsigned int hash = len, i;
	unsigned char index;

	for (i = 0; i != len; i++, str++)
	{

		index = (unsigned char)(*str) + i;

		hash =
			LROT(hash + ((hash << 8) ^ (hash >> 12))) +
			sTable[index];

	}

	return hash;
}

#ifdef DIRECT_ROTATE
#define LROT13(x) _lrotl(x, 13)
#else
#define LROT13(x) (((x) << 13) | ((x) >> 19))
#endif

DIGEST_TYPE __declspec(dllexport)
MaHash7(char *str, unsigned int len)
{
	unsigned int hash = len, i;
	unsigned char index;


	for (i = 0; i != len * ITERATIONS; i++)
	{

		index = (unsigned char)(*(str + (i % len))) + i;

		hash =
			LROT13(hash + ((hash << 8) ^ (hash >> 12))) -
			sTable[index];

	}

	return hash;
}


#ifdef DIRECT_ROTATE
#define LROTA(x) _lrotl(x, 14)
#else
#define LROTA(x) (((x) << 14) | ((x) >> 18 ))
#endif

DIGEST_TYPE __declspec(dllexport)
MaHash9(char *str, unsigned int len)
{
	unsigned int hash = len, i;
	unsigned char index;

	for (i = 0; i != len; i++, str++)
	{

		index = (unsigned char)(*str) + i;

		hash =
			LROTA(~hash + ((hash << 6) ^ (hash >> 11))) -
			sTable[index];

	}

	return hash;
}




#define LROT1(x) (((x) << 1) | ((x) >> 31 ))
#define MR_ITERATIONS 3

union type_regs
{
	unsigned long d32;
	unsigned char d8[4];
};


DIGEST_TYPE __declspec(dllexport)
MaHashMR1(char *str, unsigned int len)
{
	unsigned int i, s = 0;
	unsigned char index;
	union type_regs regs;

	regs.d32 = len;

	for (i = 0; i != len * MR_ITERATIONS; i++)
	{
		index = (unsigned char)(*(str + (i % len))) + i;

		regs.d8[s] = sTable[(sTable[index] + regs.d8[s]) & 255];

		s = (s + 1) % 4;

		regs.d32 = LROT1(regs.d32);
	}

	return regs.d32;
}



DIGEST_TYPE __declspec(dllexport)
maPrime2Hash(char *str, unsigned int len)
{
	unsigned int i, hash = 0, seed = 9999;
	unsigned char val;

	/* seed: 1919, 1717=849, 2323, 9999  */

	for (i = 0; i != len; i++, str++)
	{
		val = (unsigned char)(*str) + i;
		hash ^= sTable[val];
		hash = hash * seed;

	}

	return hash;
}


DIGEST_TYPE __declspec(dllexport)
maPrime2aHash(char *str, unsigned int len)
{
	unsigned int i, hash = 0, seed = 9999;
	unsigned char val;

	/* seed: 1919, 1717=849, 2323, 9999  */

	for (i = 0; i != len; i++, str++)
	{
		val = (unsigned char)(*str) + i;
		hash += sTable[val];
		hash = hash * seed;

	}

	return hash;
}


DIGEST_TYPE __declspec(dllexport)
maPrime2bHash(char *str, unsigned int len)
{
	unsigned int i, hash = 0, seed = 9999;
	unsigned char val;

	/* seed: 1919, 1717=849, 2323, 9999  */

	for (i = 0; i != len; i++, str++)
	{
		val = (unsigned char)(*str) + i;
		hash = hash * seed;
		hash += sTable[val];

	}

	return hash;
}



DIGEST_TYPE __declspec(dllexport)
maPrime2cHash(char *str, unsigned int len)
{
	unsigned int hash = len, i;
	unsigned char val, byte;

	for (i = 0; i != len; i++, str++)
	{
		byte = (unsigned char)(*str) + i;
		val = sTable[byte];
		hash ^= val;
		hash = hash * 1717;
	}
	return hash;
}


DIGEST_TYPE __declspec(dllexport)
maPrime2dHash(char *str, unsigned int len)
{
	unsigned int hash = 0, i;
	unsigned char val, byte;
	unsigned int rotate = 2;
	unsigned int seed = 0x1A4E41U;



	for (i = 0; i != len; i++, str++)
	{
		byte = (unsigned char)(*str) + i;
		val = sTable[byte];
		hash += val;
		hash = (hash << (32 - rotate)) | (hash >> rotate);
		hash = (hash + i) * seed;

	}


	return (hash + len) * seed;
}




DIGEST_TYPE __declspec(dllexport)
JSHash(char *str, unsigned int len)
{
	unsigned int hash = 1315423911;
	unsigned int i = 0;

	for (i = 0; i < len; str++, i++)
	{
		hash ^= ((hash << 5) + (unsigned char)(*str) + (hash >> 2));
	}
	return hash;
}



DIGEST_TYPE __declspec(dllexport)
ELFHash(char *str, unsigned int len)
{
	unsigned int hash = 0;
	unsigned int x = 0;
	unsigned int i = 0;

	for (i = 0; i < len; str++, i++)
	{
		hash = (hash << 4) + (unsigned char)(*str);
		if ((x = hash & 0xF0000000L) != 0)
		{
			hash ^= (x >> 24);
			hash &= ~x;
		}
	}
	return hash;
}

DIGEST_TYPE __declspec(dllexport)
BKDRHash(char *str, unsigned int len)
{
	unsigned int seed = 131313;   /* 31 131 1313 13131 131313 etc.. */
	unsigned int hash = 0;
	unsigned int i = 0;

	for (i = 0; i < len; str++, i++)
	{
		hash = (hash * seed) + (unsigned char)(*str);
	}
	return hash;
}

DIGEST_TYPE __declspec(dllexport)
maBKDRHash(char *str, unsigned int len)
{
	unsigned int seed = 131313;   /* 31 131 1313 13131 131313 etc.. */
	unsigned int hash = 0;
	unsigned int i = 0;

	for (i = 0; i < len; str++, i++)
	{
		hash = (hash * seed) + (unsigned char)(*str) + /* maBKDR modification here*/ i;
	}
	return hash;
}


DIGEST_TYPE __declspec(dllexport)
DEKHash(char *str, unsigned int len)
{
	unsigned int hash = len;
	unsigned int i = 0;

	for (i = 0; i < len; str++, i++)
	{
		hash = ((hash << 5) ^ (hash >> 27)) ^ (unsigned char)(*str);
	}
	return hash;
}


DIGEST_TYPE __declspec(dllexport)
APHash(char *str, unsigned int len)
{
	unsigned int hash = 0;
	unsigned int i = 0;

	for (i = 0; i < len; str++, i++)
	{
		hash ^= ((i & 1) == 0) ?
			((hash << 7) ^ (unsigned char)(*str) ^ (hash >> 3)) :
			(~((hash << 11) ^ (unsigned char)(*str) ^ (hash >> 5)));
	}
	return hash;
}



DIGEST_TYPE __declspec(dllexport)
LYHash(char *str, unsigned int len)
{
	unsigned int hash = 0;
	unsigned int i = 0;

	for (i = 0; i < len; str++, i++)
	{
		hash = (hash * 1664525) + (unsigned char)(*str) + 1013904223;
	}
	return hash;
}


DIGEST_TYPE __declspec(dllexport)
bob_faq6_hash(char *str, unsigned int len)
{
	unsigned int hash = 0;
	unsigned int i = 0;

	for (i = 0; i < len; str++, i++)
	{
		hash += (unsigned char)(*str);
		hash += (hash << 10);
		hash ^= (hash >> 6);
	}
	hash += (hash << 3);
	hash ^= (hash >> 11);
	hash += (hash << 15);
	return hash;
}


#define rot(x,k) (((x)<<(k)) ^ ((x)>>(32-(k))))

#define mix(a,b,c) { \
	a -= c;  a ^= rot(c, 4);  c += b; \
	b -= a;  b ^= rot(a, 6);  a += c; \
	c -= b;  c ^= rot(b, 8);  b += a; \
	a -= c;  a ^= rot(c, 16);  c += b; \
	b -= a;  b ^= rot(a, 19);  a += c; \
	c -= b;  c ^= rot(b, 4);  b += a; }

#define final(a,b,c) { \
	c ^= b; c -= rot(b, 14); \
	a ^= c; a -= rot(c, 11); \
	b ^= a; b -= rot(a, 25); \
	c ^= b; c -= rot(b, 16); \
	a ^= c; a -= rot(c, 4);  \
	b ^= a; b -= rot(a, 14); \
	c ^= b; c -= rot(b, 24); }

DIGEST_TYPE __declspec(dllexport)
bob_lookup3_hash(char *str, unsigned int len8)
{
	unsigned int a, b, c;

	/* the key, an array of unsigned int values */
	unsigned int *k = (unsigned int *)str;

	/* the length of the key, in unsigned ints */
	unsigned int length = (len8 + 3) / 4;

	/* tail pad with zeros */
	str[len8] = 0;
	str[len8 + 1] = 0;
	str[len8 + 2] = 0;

	/* Set up the internal state */
	a = b = c = 0xdeadbeef + len8;

	/* handle most of the key */
	while (length > 3)
	{
		a += k[0];
		b += k[1];
		c += k[2];
		mix(a, b, c);
		length -= 3;
		k += 3;
	}

	/* handle the last 3 unsigned ints */
	switch (length)
	{
	case 3:
		c += k[2];
	case 2:
		b += k[1];
	case 1:
		a += k[0];
		final(a, b, c);
	case 0:
		/* nothing left to add */
		break;
	}
	return c;
}


int crc_init = 0;

unsigned int crc_table[256];

void
crc32gentab(void)
{
	unsigned long crc, poly;
	int i, j;

	poly = 0xEDB88320L;
	for (i = 0; i < 256; i++)
	{
		crc = i;
		for (j = 8; j > 0; j--)
		{
			if (crc & 1)
			{
				crc = (crc >> 1) ^ poly;
			}
			else
			{
				crc >>= 1;
			}
		}
		crc_table[i] = crc;
	}
}


DIGEST_TYPE __declspec(dllexport)
crc32(char *buf, unsigned int len)
{
	unsigned int crc;

	if (crc_init == 0)
	{
		crc32gentab();
		crc_init = 1;
	}

	crc = 0xffffffff;
	while (len--)
	{
		crc = crc_table[(crc ^ (unsigned char)*buf++) & 0xff] ^ (crc >> 8);
	}
	return crc ^ 0xffffffff;
}




DIGEST_TYPE __declspec(dllexport) SDBMHash(char* str, unsigned int len)
{
	unsigned int hash = 0;
	unsigned int i = 0;

	for (i = 0; i < len; str++, i++)
	{
		hash = (*str) + (hash << 6) + (hash << 16) - hash;
	}

	return hash;
}


DIGEST_TYPE __declspec(dllexport) DJBHash(char* str, unsigned int len)
{
	unsigned int hash = 5381;
	unsigned int i = 0;

	for (i = 0; i < len; str++, i++)
	{
		hash = ((hash << 5) + hash) + (*str);
	}

	return hash;
}


DIGEST_TYPE __declspec(dllexport) BPHash(char* str, unsigned int len)
{
	unsigned int hash = 0;
	unsigned int i = 0;
	for (i = 0; i < len; str++, i++)
	{
		hash = hash << 7 ^ (*str);
	}

	return hash;
}


DIGEST_TYPE __declspec(dllexport) FNVHash(char* str, unsigned int len)
{
	const unsigned int fnv_prime = 0x811C9DC5;
	unsigned int hash = 0;
	unsigned int i = 0;

	for (i = 0; i < len; str++, i++)
	{
		hash *= fnv_prime;
		hash ^= (*str);
	}

	return hash;
}


#define FNV_32_PRIME ((unsigned int)0x01000193)

DIGEST_TYPE __declspec(dllexport) FNV1Hash(char *buf, unsigned int len)
{
	unsigned char *bp = (unsigned char *)buf;       /* start of buffer */
	unsigned char *be = bp + len;           /* beyond end of buffer */
	unsigned int hval = 0x811c9dc5;

	/*
	* FNV-1 hash each octet in the buffer
	*/
	while (bp < be)
	{

		/* multiply by the 32 bit FNV magic prime mod 2^32 */

		hval *= FNV_32_PRIME;

		/* xor the bottom with the current octet */
		hval ^= (unsigned int)*bp++;
	}

	/* return our new hash value */
	return hval;
}


DIGEST_TYPE __declspec(dllexport) FNV1aHash(char *buf, unsigned int len)
{
	unsigned char *bp = (unsigned char *)buf;       /* start of buffer */
	unsigned char *be = bp + len;           /* beyond end of buffer */
	unsigned int hval = 0x811c9dc5;

	/*
	* FNV-1 hash each octet in the buffer
	*/
	while (bp < be)
	{


		/* xor the bottom with the current octet */
		hval ^= (unsigned int)*bp++;

		/* multiply by the 32 bit FNV magic prime mod 2^32 */

		hval *= FNV_32_PRIME;

	}

	/* return our new hash value */
	return hval;
}


#define PRIME_MULT 0x1FAF
#define START_PRIME 0x3A8F05C5
#define USE_SBOX

DIGEST_TYPE __declspec(dllexport) maPrimeHash(char *buf, unsigned int len)
{
	unsigned int hval = START_PRIME, i;
	unsigned char index;

	for (i = 0; i != len; i++, buf++)
	{

		index = (unsigned char)*buf;

#ifdef USE_SBOX
		hval ^= sTable[(index + i) & 255];
#else
		hval += index  ^  i;
#endif


		hval *= PRIME_MULT;

	}


	return hval;
}



const unsigned char novak_table[256] =
{
	0x63, 0x7c, 0x77, 0x7b, 0xf2, 0x6b, 0x6f, 0xc5,
	0x30, 0x01, 0x67, 0x2b, 0xfe, 0xd7, 0xab, 0x76,
	0xca, 0x82, 0xc9, 0x7d, 0xfa, 0x59, 0x47, 0xf0,
	0xad, 0xd4, 0xa2, 0xaf, 0x9c, 0xa4, 0x72, 0xc0,
	0xb7, 0xfd, 0x93, 0x26, 0x36, 0x3f, 0xf7, 0xcc,
	0x34, 0xa5, 0xe5, 0xf1, 0x71, 0xd8, 0x31, 0x15,
	0x04, 0xc7, 0x23, 0xc3, 0x18, 0x96, 0x05, 0x9a,
	0x07, 0x12, 0x80, 0xe2, 0xeb, 0x27, 0xb2, 0x75,
	0x09, 0x83, 0x2c, 0x1a, 0x1b, 0x6e, 0x5a, 0xa0,
	0x52, 0x3b, 0xd6, 0xb3, 0x29, 0xe3, 0x2f, 0x84,
	0x53, 0xd1, 0x00, 0xed, 0x20, 0xfc, 0xb1, 0x5b,
	0x6a, 0xcb, 0xbe, 0x39, 0x4a, 0x4c, 0x58, 0xcf,
	0xd0, 0xef, 0xaa, 0xfb, 0x43, 0x4d, 0x33, 0x85,
	0x45, 0xf9, 0x02, 0x7f, 0x50, 0x3c, 0x9f, 0xa8,
	0x51, 0xa3, 0x40, 0x8f, 0x92, 0x9d, 0x38, 0xf5,
	0xbc, 0xb6, 0xda, 0x21, 0x10, 0xff, 0xf3, 0xd2,
	0xcd, 0x0c, 0x13, 0xec, 0x5f, 0x97, 0x44, 0x17,
	0xc4, 0xa7, 0x7e, 0x3d, 0x64, 0x5d, 0x19, 0x73,
	0x60, 0x81, 0x4f, 0xdc, 0x22, 0x2a, 0x90, 0x88,
	0x46, 0xee, 0xb8, 0x14, 0xde, 0x5e, 0x0b, 0xdb,
	0xe0, 0x32, 0x3a, 0x0a, 0x49, 0x06, 0x24, 0x5c,
	0xc2, 0xd3, 0xac, 0x62, 0x91, 0x95, 0xe4, 0x79,
	0xe7, 0xc8, 0x37, 0x6d, 0x8d, 0xd5, 0x4e, 0xa9,
	0x6c, 0x56, 0xf4, 0xea, 0x65, 0x7a, 0xae, 0x08,
	0xba, 0x78, 0x25, 0x2e, 0x1c, 0xa6, 0xb4, 0xc6,
	0xe8, 0xdd, 0x74, 0x1f, 0x4b, 0xbd, 0x8b, 0x8a,
	0x70, 0x3e, 0xb5, 0x66, 0x48, 0x03, 0xf6, 0x0e,
	0x61, 0x35, 0x57, 0xb9, 0x86, 0xc1, 0x1d, 0x9e,
	0xe1, 0xf8, 0x98, 0x11, 0x69, 0xd9, 0x8e, 0x94,
	0x9b, 0x1e, 0x87, 0xe9, 0xce, 0x55, 0x28, 0xdf,
	0x8c, 0xa1, 0x89, 0x0d, 0xbf, 0xe6, 0x42, 0x68,
	0x41, 0x99, 0x2d, 0x0f, 0xb0, 0x54, 0xbb, 0x16
};




DIGEST_TYPE __declspec(dllexport) NovakHash(char *buf, unsigned int len)
{

	unsigned int h = 1, i;
	unsigned char * t = (unsigned char *)buf;
	for (i = 0; i < (len & ~1); i += 2)
	{
		h += (h << 1) + novak_table[t[i]];
		h += (h << 1) + novak_table[t[i + 1]];
	}
	if (len & 1)
		h += (h << 1) + novak_table[t[len - 1]];
	return h;
}


DIGEST_TYPE __declspec(dllexport) FletcherHash(char *key, unsigned int len)
{
	const unsigned short * data = (const unsigned short *)key;
	unsigned int sum1 = 0xFFFF, sum2 = 0xFFFF;

	len /= 2;

	while (len)
	{
		unsigned int tlen = len > 360 ? 360 : len;
		len -= tlen;
		do
		{
			sum1 += *data++;
			sum2 += sum1;
		} while (--tlen);
		sum1 = (sum1 & 0xffff) + (sum1 >> 16);
		sum2 = (sum2 & 0xffff) + (sum2 >> 16);
	}
	/* Second reduction step to reduce sums to 16 bits */
	sum1 = (sum1 & 0xffff) + (sum1 >> 16);
	sum2 = (sum2 & 0xffff) + (sum2 >> 16);
	return sum2 << 16 | sum1;
}


DIGEST_TYPE __declspec(dllexport) AdlerHash(char *str, unsigned int len)
{
	unsigned int a = 1, b = 0;
	unsigned char *data = (unsigned char *)str;

	while (len > 0)
	{
		unsigned int tlen = len > 5550 ? 5550 : len;
		len -= tlen;
		do
		{
			a += *data++;
			b += a;
		} while (--tlen);

		a %= 65521;
		b %= 65521;
	}
	return (b << 16) | a;
}


DIGEST_TYPE __declspec(dllexport) x31Hash(char *str, unsigned int len)
{
	unsigned int h = len;
	unsigned char *key = (unsigned char *)str;

	for (; len &~1; len -= 2, key += 2)
		h = (((h * 31) + key[0]) * 31) + key[1];
	if (len & 1)
		h = (h * 31) + key[0];
	return h ^ (h >> 16);
};



DIGEST_TYPE __declspec(dllexport) x17Hash(char *str, unsigned int len)
{
	unsigned int h = len;
	unsigned char *key = (unsigned char *)str;

	for (; len &~1; len -= 2, key += 2)
		h = (((h * 17) + (key[0] - ' ')) * 17) + (key[1] - ' ');
	if (len & 1)
		h = (h * 17) + (key[0] - ' ');
	return h ^ (h >> 16);
};




void
doHash(char *line, int pos, enum HashTypes type)
{

	DIGEST_TYPE(*HashFunc)(char *, unsigned int) = NULL;



	switch (type)

	{
	case ht_ROT13Hash:
		HashFunc = ROT13Hash;
		break;


	case ht_RSHash:
		HashFunc = RSHash;
		break;


	case ht_PJWHash:
		HashFunc = PJWHash;
		break;

	case ht_maPrime:
		HashFunc = maPrimeHash;
		break;

	case ht_maPrime2:
		HashFunc = maPrime2Hash;
		break;

	case ht_maPrime2a:
		HashFunc = maPrime2aHash;
		break;

	case ht_maPrime2b:
		HashFunc = maPrime2bHash;
		break;

	case ht_maPrime2c:
		HashFunc = maPrime2cHash;
		break;

	case ht_maPrime2d:
		HashFunc = maPrime2dHash;
		break;

	case ht_SuperFastHash:
		HashFunc = SuperFastHash;
		break;

	case ht_OneAtTimeHash:
		HashFunc = OneAtTimeHash;
		break;

	case ht_GoulburnHash:
		HashFunc = GoulburnHash;
		break;

	case ht_SBOXHash:
		HashFunc = SBOXHash;
		break;

	case ht_Crap8Hash:
		HashFunc = Crap8Hash;
		break;

	case ht_CrapWowHash:
		HashFunc = CrapWowHash;
		break;

	case ht_NovakHash:
		HashFunc = NovakHash;
		break;

	case ht_AdlerHash:
		HashFunc = AdlerHash;
		break;

	case ht_FletcherHash:
		HashFunc = FletcherHash;
		break;

	case ht_x17Hash:
		HashFunc = x17Hash;
		break;


	case ht_x31Hash:
		HashFunc = x31Hash;
		break;


	case ht_maHashMR1:
		HashFunc = MaHashMR1;
		break;

	case ht_MaHash:
		HashFunc = MaHash;
		break;


	case ht_MaHash1:
		HashFunc = MaHash1;
		break;


	case ht_MaHash3:
		HashFunc = MaHash3;
		break;

	case ht_MaHash2:
		HashFunc = MaHash2;
		break;

	case ht_MaHash11:
		HashFunc = MaHash11;
		break;

	case ht_MaHash4:
		HashFunc = MaHash4;
		break;

	case ht_MaHash4v64:
		HashFunc = MaHash4v64;
		break;

	case ht_MaHash8:
		HashFunc = MaHash8;
		break;

	case ht_MaHash8v64:
		HashFunc = MaHash8v64;
		break;

	case ht_MaHash9:
		HashFunc = MaHash9;
		break;

	case ht_MaHash10:
		HashFunc = MaHash10;
		break;


	case ht_MaHash5:
		HashFunc = MaHash5;
		break;

	case ht_MaHash6:
		HashFunc = MaHash6;
		break;

	case ht_MaHash7:
		HashFunc = MaHash7;
		break;


	case ht_JSHash:
		HashFunc = JSHash;
		break;

	case ht_ELFHash:
		HashFunc = ELFHash;
		break;

	case ht_BKDRHash:
		HashFunc = BKDRHash;
		break;

	case ht_maBKDRHash:
		HashFunc = maBKDRHash;
		break;

	case ht_DEKHash:
		HashFunc = DEKHash;
		break;

	case ht_APHash:
		HashFunc = APHash;
		break;

	case ht_LYHash:
		HashFunc = LYHash;
		break;

	case ht_bob_faq6_hash:
		HashFunc = bob_faq6_hash;
		break;

	case ht_bob_lookup3_hash:
		HashFunc = bob_lookup3_hash;
		break;

	case ht_crc32:
		HashFunc = crc32;
		break;

	case ht_SDBMHash:
		HashFunc = SDBMHash;
		break;

	case ht_DJBHash:
		HashFunc = DJBHash;
		break;

	case ht_BPHash:
		HashFunc = BPHash;
		break;

	case ht_FNVHash:
		HashFunc = FNVHash;
		break;

	case ht_FNV1Hash:
		HashFunc = FNV1Hash;
		break;

	case ht_FNV1aHash:
		HashFunc = FNV1aHash;
		break;

	case ht_MurmurHash2:
		HashFunc = MurmurHash2;
		break;

	case ht_MurmurHash2A:
		HashFunc = MurmurHash2A;
		break;

	case ht_MurmurHash2AM:
		HashFunc = MurmurHash2AM;
		break;

	case ht_MurmurHash3:
		HashFunc = MurmurHash3;
		break;


	default:
		fprintf(stdout, "Hash not found!");
		return;
		break;

	}


	HashTab[pos].digest = HashFunc(line, strlen(line));
	HashTab[pos].used = 1;
	SortTab[pos] = HashTab[pos].digest;
	/*
	strcpy (HashTab[pos].text, line);
	*/

}


int
compare(const void *a, const void *b)
{
	return (int)(*(DIGEST_TYPE *)a - *(DIGEST_TYPE *)b);
}



int
CalcCollisions(void)
{

	int col = 0, i;
	DIGEST_TYPE last = 0;


	qsort(SortTab, NUM_WORDS, sizeof (DIGEST_TYPE), compare);

	for (i = 0; i != NUM_WORDS; i++)
	{



		if (i != 0)
		{

			if (last == SortTab[i])
			{
				col++;
			}

		}

		last = SortTab[i];


	}

	return col;

}