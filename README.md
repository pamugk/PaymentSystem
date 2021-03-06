# Сервер платёжной системы

## Текущее состояние релиза

![GitHub Workflow Status](https://img.shields.io/github/workflow/status/pamugk/PaymentSystem/.NET%20Core?logo=.Net)
![GitHub Workflow Status](https://img.shields.io/github/workflow/status/pamugk/PaymentSystem/Docker?label=docker%20build&logo=Docker)

## О размещении в Интернете

Контейнеры, формируемые для сервера, хранятся в [этом](https://hub.docker.com/repository/docker/pamugk/paymentsystem "Ссылка на репозиторий DockerHub") репозитории.  
Сервер размещён на платформе Heroku вот по [этому](https://payment--system.herokuapp.com) адресу.  
Соответственно, OpenAPI спецификация доступна вот [тут](https://payment--system.herokuapp.com/swagger).

## О сервере

Данный сервер предоставляет API, имитирующее платёжную систему (создано исключительно в учебных целях и не предназначено для использования в реальных коммерческих системах).  
Так, имеется возможность запросить сессию для платежа (должны быть указаны сумма и назначение платежа), а затем осуществить имитацию совершения платежа - для этого достаточно отправить POST-запрос с указанием данных условной банковской карты, идентификатором сессии и URL, на которое надо отправить уведомление (опционально). Если номер банковской карты проходит проверку упрощённым алгоритмом Луна, то сервер делает вид, что провёл платёж, иначе считается, что платёж не был осуществлён. Платежи осуществляются только для валидных сессий (валидна сессия, которая зарегистрирована в системе, не достигшая конца своего срока действия и при этом имеющая несовершённый платёж).  
Стоит отметить, что пока отправка уведомления осуществляется очень условно (точнее не осуществляется вовсе - уведомление отправляется прямиком в консоль, и никаких HTTP-запросов не осуществляется).  
Также имеется возможность получения списка платежей за определённый период (платёж считается принадлежащим к той дате, когда была запрошена для него сессия). Этот метод требует авторизации (а перед авторизацией пользователю ещё необходимо и зарегистрироваться в системе, куда ж без этого).  

## OpenAPI-спецификация

Полное описание API, предоставляемого сервером, можно найти по адресу `/swagger/v1/swagger.json` (путь указан относительно адреса, по которому развёрнут сервер). Графический интерфейс для ознакомления со спецификацией API можно найти по адресу `/swagger` (опять же, путь относительный).

## Технологии

Было использовано две технологии: ASP.NET Core и Docker. С использованием ASP.NET Core было разработано приложение сервера, а Docker был использован для упаковки приложения сервера в контейнер.  
Для написания тестов был использован [xUnit](https://xunit.net/ "Сайт xUnit"). Тестами покрыты все основные сервисы, используемые сервером.  
OpenAPI-спецификация генерируется с помощью [Swashbuckle.AspNetCore](https://github.com/domaindrivendev/Swashbuckle.AspNetCore "Репозиторий Swashbuckle.AspNetCore") на основе программного кода сервера, что весьма удобно. Сгенерированная спецификация и веб-страница для её просмотра выставляются сервером по указанным ранее адресам.
