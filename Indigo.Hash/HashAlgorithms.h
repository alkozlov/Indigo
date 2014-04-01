#include <stdio.h>
#include <stdlib.h>
#include <string.h>

typedef unsigned long long DIGEST_TYPE;

DIGEST_TYPE __declspec(dllexport) GoulburnHash(char *cp, unsigned int len);