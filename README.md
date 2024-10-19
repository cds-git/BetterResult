# BetterResult
Just my personal implementation of the result pattern to get better understanding of it.

## TODO:
- Add example usage to README
- Add extensive testing
- Add existing operations to the base result class
- Add helper methods for interacting with metadata in erorrs
    - Add, Remove, Override, Extract by type
        - WithMetadata, WithMessage, `T? TryGet<T>()`
- Either extending Error or making a Result hold multiple Errors

## Credits - Inspiration
[ErrorOr](https://github.com/amantinband/error-or)
[FluentResults](https://github.com/altmann/FluentResults)
[OneOf](https://github.com/mcintyre321/OneOf)

