FOR /L %%i in (1, 1, %1) do @if %%i LEQ 9 @( echo %%i localhost:500%%i >> test.txt
) else @echo %%i localhost:50%%i >> test.txt