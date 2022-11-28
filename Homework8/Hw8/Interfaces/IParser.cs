using Hw8.Common;

namespace Hw8.Interfaces;

public interface IParser
{
    ParseOutput ParseCalcArguments(ParseInput input);
}