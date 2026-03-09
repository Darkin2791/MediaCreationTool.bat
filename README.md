# OfferBuilder (Windows Desktop, WPF)

Приложение для менеджеров по быстрому созданию, редактированию и экспорту коммерческих предложений в PDF (на русском языке).

## Технологический стек
- C# / .NET 8
- WPF + MVVM
- CommunityToolkit.Mvvm
- SQLite
- QuestPDF
- Serilog

## Структура решения
- `src/OfferBuilder.Domain` — доменные сущности (`Offer`, `OfferSection`, `Template`, каталоги и пр.)
- `src/OfferBuilder.Application` — интерфейсы репозиториев/сервисов, бизнес-логика (`PriceCalculationService`, `SectionOrderingService`), ViewModel
- `src/OfferBuilder.Infrastructure` — SQLite-репозитории, seed-данные, PDF/preview рендеринг
- `src/OfferBuilder.UI` — WPF UI: список КП, редактирование, секции, предпросмотр, экспорт
- `src/OfferBuilder.Tests` — unit-тесты калькуляции цены и порядка секций

## MVP (Phase 1) реализовано
- Создание/сохранение предложения
- Список предложений
- Модульные секции с `SortOrder`, `IsVisible`, ручным редактированием
- Хранение данных в SQLite
- Seed демо данных на русском
- Два шаблона:
  - «Стандартный бизнес»
  - «Визуальное КП по строительству дома»
- Предпросмотр через тот же PDF рендерер (единая логика layout)
- Экспорт PDF

## Запуск
1. Установите .NET SDK 8 и Visual Studio 2022 (Desktop development with .NET).
2. Откройте `OfferBuilder.sln`.
3. Установите NuGet зависимости.
4. Запустите `OfferBuilder.UI`.

При первом запуске:
- создаётся `offerbuilder.db`
- выполняется инициализация схемы
- загружаются demo шаблоны и каталоги

## Архитектурные заметки
- Слои разделены по ответственности (UI/Application/Domain/Infrastructure).
- `OfferSection.ContentJson` используется как гибкое JSON-хранилище контента блока.
- PDF и Preview используют общую логику через `IPdfRenderService`/`IPreviewRenderService`.
- DI на `Microsoft.Extensions.DependencyInjection`.
- Логирование в `logs/app.log`.

## Ограничения текущего MVP
- UI вкладки реализованы базово (General / Sections / Preview), остальные расширяются во Phase 2.
- Drag&drop reorder, undo/redo, autosave и конструктор шаблонов запланированы в Phase 2.
- В headless Linux-среде WPF не запускается; проект рассчитан на Windows.
