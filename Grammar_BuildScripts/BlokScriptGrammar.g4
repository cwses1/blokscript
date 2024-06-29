grammar BlokScriptGrammar;

@header {#pragma warning disable 3021}

STATEMENTEND: ';';
WS: [ \r\t\n]+ -> skip ;
STRINGLITERAL: '\'' ([ a-zA-Z0-9%_/-] | '.' | '#')* '\'';
VARID: [a-zA-Z][a-zA-Z0-9_]+;
INTLITERAL: [0-9]+;
REGEXLITERAL: '/' ('^' | '$' | '*' | '[' | ']' | [a-z]+ | [A-Z]+ | [0-9]+ | '-' | '(' | ')' | '.' | '+' | '\\/')* '/';
LINE_COMMENT: '//' ~( '\n'|'\r' )* '\r'? '\n' -> skip;
BLOCK_COMMENT: '/*' .*? '*/' -> skip;

script: statementList;

statementList: statement*;

statement: loginStatement STATEMENTEND
	| varStatement STATEMENTEND
	| assignmentStatement STATEMENTEND
	| printStatement STATEMENTEND
	| verbosityStatement STATEMENTEND
	| waitStatement STATEMENTEND
	| compareStatement STATEMENTEND
	
	| createSpaceStatement STATEMENTEND
	| copySpaceStatement STATEMENTEND
	| updateSpaceStatement STATEMENTEND
	| deleteSpaceStatement STATEMENTEND

	| copySpacesStatement STATEMENTEND

	| createBlockStatement STATEMENTEND
	| copyBlockStatement STATEMENTEND
	| updateBlockStatement STATEMENTEND
	| deleteBlockStatement STATEMENTEND

	| copyBlocksStatement STATEMENTEND
	| deleteBlocksStatement STATEMENTEND

	| createDatasourceStatement STATEMENTEND
	| copyDatasourceStatement STATEMENTEND
	| updateDatasourceStatement STATEMENTEND
	| deleteDatasourceStatement STATEMENTEND

	| copyDatasourcesStatement STATEMENTEND
	| deleteDatasourcesStatement STATEMENTEND
	| updateDatasourcesStatement STATEMENTEND
	
	| copyStoriesStatement STATEMENTEND
	| publishStoriesStatement STATEMENTEND
	| unpublishStoriesStatement STATEMENTEND
	| deleteStoriesStatement STATEMENTEND
	
	| createDatasourceEntryStatement STATEMENTEND
	| copyDatasourceEntryStatement STATEMENTEND
	| deleteDatasourceEntryStatement STATEMENTEND
	| updateDatasourceEntryStatement STATEMENTEND

	| updateDatasourceEntriesStatement STATEMENTEND
	| deleteDatasourceEntriesStatement STATEMENTEND
	| copyDatasourceEntriesStatement STATEMENTEND
	| syncDatasourceEntriesStatement STATEMENTEND
	
	| 'pass' STATEMENTEND
	| scriptBlockDef
	| forEachStatement
	;

createSpaceStatement: 'create' 'space' (stringExpr | '(' spaceUpdateList ')');
copySpaceStatement: 'copy' 'space' longOrShortSpaceSpec 'to' spaceOutputLocation;
updateSpaceStatement: 'update' 'space' longOrShortSpaceSpec 'set' spaceUpdateList;
deleteSpaceStatement: 'delete' 'space' longOrShortSpaceSpec;
spaceUpdateList: spaceUpdate (',' spaceUpdateList)?;
spaceUpdate: 'name' '=' stringExpr
	| 'server' 'location' '=' stringExpr
	| 'default' 'content' 'type' '=' stringExpr
	;

copySpacesStatement: 'copy' 'spaces' ('from' spacesInputLocation)? 'to' spacesOutputLocation;
selectSpacesStatement: 'select' selectFieldList 'from' constrainedSpaceList ('to' spacesOutputLocation)?;

selectFieldList: '*' (',' selectFieldList)?
	| VARID (',' selectFieldList)?
	| VARID
	;

constrainedSpaceList: completeSpaceList ('where' spaceConstraintExprList)?;
completeSpaceList: 'spaces' (('from' | 'in') spacesInputLocation)?;

createBlockStatement: 'create' 'block' '(' blockUpdateList ')' 'in' longOrShortSpaceSpec;
copyBlockStatement: 'copy' 'block' longOrShortBlockSpec 'to' blockOutputLocation;
updateBlockStatement: 'update' 'block' longOrShortBlockSpec 'set' blockUpdateList;
deleteBlockStatement: 'delete' 'block' longOrShortBlockSpec;
longOrShortBlockSpec: blockSpec | shortBlockSpec;
shortBlockSpec: (stringExpr | VARID) 'in' longOrShortSpaceSpec
	| VARID
	;

scriptBlockDef: '{' statementList? '}';

createDatasourceStatement: 'create' 'datasource' (stringExpr | '(' datasourceUpdateList ')') ('for' | 'in') (spaceSpec | shortSpaceSpec);
copyDatasourceStatement: 'copy' 'datasource' longOrShortDatasourceSpec 'to' datasourceOutputLocation;
deleteDatasourceStatement: 'delete' 'datasource' (datasourceShortSpec | datasourceSpec);
updateDatasourceStatement: 'update' 'datasource' (datasourceShortSpec | datasourceSpec) 'set' datasourceUpdateList;

datasourceUpdateList: datasourceUpdate (',' datasourceUpdateList)?;

datasourceUpdate: 'name' '=' stringExpr
	| 'slug' '=' stringExpr
	;

createDatasourceEntryStatement: 'create' 'datasource' 'entry' (stringExpr | datasourceEntryUpdateList) ('for' | 'in') (datasourceSpec | datasourceShortSpec);
copyDatasourceEntryStatement: 'copy' 'datasource' 'entry' longOrShortDatasourceEntrySpec 'to' datasourceEntryOutputLocation;
deleteDatasourceEntryStatement: 'delete' 'datasource' 'entry' datasourceEntryShortSpec;
updateDatasourceEntryStatement: 'update' 'datasource' 'entry' datasourceEntryShortSpec 'set' datasourceEntryUpdateList;

longOrShortDatasourceEntrySpec: datasourceEntryFullSpec | datasourceEntryShortSpec;
datasourceEntryFullSpec: 'datasource' 'entry' datasourceEntryIdentifier ('from' | 'in') datasourceSpec;
datasourceEntryShortSpec: datasourceEntryIdentifier ('from' | 'in') datasourceSpec;
datasourceEntryIdentifier: (intExpr | stringExpr | VARID);
datasourceEntryOutputLocation: longOrShortDatasourceSpec | completeFileSpec;

updateDatasourceEntriesStatement: 'update' 'datasource' 'entries' 'in' datasourceSpec 'set' datasourceEntryUpdateList ('where' datasourceEntryConstraintExprList)?;
deleteDatasourceEntriesStatement: 'delete' 'datasource' 'entries' ('from' | 'in') (datasourceSpec | datasourceShortSpec) ('where' datasourceEntryConstraintExprList)?;
copyDatasourceEntriesStatement: 'copy' 'datasource' 'entries' ('from' | 'in') datasourceEntriesSourceLocation 'to' datasourceEntriesTargetLocation ('where' datasourceEntryConstraintExprList)? datasourceEntryCopyOptionList?;
syncDatasourceEntriesStatement: 'sync' 'datasource' 'entries' ('from' | 'in') datasourceEntriesSourceLocation 'to' datasourceEntriesSourceLocation ('where' datasourceEntryConstraintExprList)?;

datasourceEntryCopyOptionList: datasourceEntryCopyOption (',' datasourceEntryCopyOptionList)?;
datasourceEntryCopyOption: 'skip' ('update' | 'updates' | 'create' | 'creates');

datasourceEntryUpdateList: datasourceEntryUpdate (',' datasourceEntryUpdateList)?;

datasourceEntryUpdate: 'name' '=' stringExpr
	| 'value' '=' stringExpr
	;

datasourceEntriesSourceLocation: longOrShortDatasourceSpec | completeFileSpec;

urlSpec: ('csv' | 'json')? 'url' stringExpr;

datasourceEntriesTargetLocation: longOrShortDatasourceSpec | completeFileSpec;
	
datasourceEntryConstraintExprList: datasourceEntryConstraintExpr (('and' | 'or') datasourceEntryConstraintExprList)?;

datasourceEntryConstraintExpr: datasourceEntryConstraint (('and' | 'or') datasourceEntryConstraintExpr)?
	| '(' datasourceEntryConstraint (('and' | 'or') datasourceEntryConstraintExpr)? ')'
	| '(' datasourceEntryConstraintExpr (('and' | 'or') datasourceEntryConstraintExpr)? ')'
	;

datasourceEntryConstraint: 'id' ('=' | '!=') intExpr
	| 'id' 'not'? 'in' '(' intExprList ')'
	| ('name' | 'value') ('=' | '!=') stringExpr
	| ('name' | 'value') 'not'? 'in' '(' (stringExprList | regexExprList) ')'
	| ('name' | 'value') ('matches' | 'does' 'not' 'match') 'regex'? regexExpr
	| ('name' | 'value') 'not'? 'in' '(' stringExprList ')'
	| ('name' | 'value') 'not'? 'like' stringExpr
	| ('name' | 'value') ('starts' | 'does' 'not' 'start') 'with' stringExpr
	| ('name' | 'value') ('ends' | 'does' 'not' 'end') 'with' stringExpr
	;


loginStatement: loginOnlyStatement 
	| loginWithGlobalUserNameStatement
	| loginWithGlobalPasswordStatement
	| loginWithGlobalTokenStatement
	| loginWithGlobalUserNameAndPasswordStatement
	;

loginOnlyStatement: 'login';
loginWithGlobalUserNameStatement: 'login' 'with' 'global' 'username';
loginWithGlobalPasswordStatement: 'login' 'with' 'global' 'password';
loginWithGlobalTokenStatement: 'login' 'with' 'global' 'token';
loginWithGlobalUserNameAndPasswordStatement: 'login' 'with' 'global' 'username' 'and' 'password';

varStatement: spaceVarStatement
	| blockVarStatement
	| stringVarStatement
	| regexVarStatement
	| storyVarStatement
	| datasourceEntryVarStatement
	| 'var' VARID '=' (VARID | spaceSpec | blockSpec | stringExpr | regexExpr | storySpec | intExpr | datasourceEntrySpec | datasourceSpec)
	;

spaceVarStatement: 'space' VARID ('=' spaceSpec)?;
blockVarStatement: 'block' VARID ('=' blockSpec)?;
stringVarStatement: 'string' VARID ('=' stringExpr)?;
regexVarStatement: 'regex' VARID ('=' regexExpr)?;
storyVarStatement: 'story' VARID ('=' storySpec)?;
datasourceEntryVarStatement: 'datasource' 'entry' VARID ('=' datasourceEntrySpec)?;

spaceSpec: 'space' (INTLITERAL | STRINGLITERAL | VARID) (varGetFrom)?
	| VARID
	;

shortSpaceSpec: INTLITERAL | STRINGLITERAL;

longOrShortSpaceSpec: spaceSpec | shortSpaceSpec;

blockSpec: 'block' STRINGLITERAL 'in' (spaceSpec | fileSpec)
	| 'block' VARID
	;

storySpec: (VARID | INTLITERAL | STRINGLITERAL) ('in' | 'from') (spaceSpec | fileSpec)
	| VARID
	;

datasourceEntrySpec: 'datasource' 'entry' (intExpr | stringExpr | VARID) ('from' | 'in') datasourceSpec
	| VARID
	;

datasourceSpec: 'datasource' (VARID | INTLITERAL | STRINGLITERAL) ('from' | 'in') (spaceSpec | shortSpaceSpec)
	| VARID
	;

datasourceShortSpec: (VARID | INTLITERAL | STRINGLITERAL) 'in' (spaceSpec | shortSpaceSpec);

assignmentStatement: VARID '=' VARID
	| spaceAssignmentStatement
	| stringAssignmentStatement
	| blockAssignmentStatement
	;

spaceAssignmentStatement: VARID '=' spaceSpec;
blockAssignmentStatement: VARID '=' blockSpec;
stringAssignmentStatement: VARID '=' STRINGLITERAL;
	

printStatement: printSpacesStatement
	| printVarStatement
	| printSpaceStatement
	| printStringLiteralStatement
	| printSymbolTableStatement
	| printLocalCacheStatement
	;

printSpacesStatement: 'print' 'spaces' 'from' realDataLocation;
printVarStatement: 'print' VARID;
printSpaceStatement: 'print' 'space' (VARID | STRINGLITERAL);
printStringLiteralStatement: 'print' STRINGLITERAL;
printSymbolTableStatement: 'print' 'symbol' 'tables';
printLocalCacheStatement: 'print' 'local' 'cache';

realDataLocation: ('server' | 'local' 'cache');

fileSpec: 'file' stringExpr;
completeFileSpec: ('csv' | 'json')? 'file' stringExpr;

spaceInputLocation: fileSpec;
spaceOutputLocation: fileSpec;

spacesInputLocation: fileSpec;
spacesOutputLocation: fileSpec | shortFileSpec;

shortFileSpec: stringExpr;

blockInputLocation: fileSpec | longOrShortSpaceSpec;
blockOutputLocation: fileSpec | longOrShortSpaceSpec;

blocksInputLocation: fileSpec | longOrShortSpaceSpec;
blocksOutputLocation: fileSpec | longOrShortSpaceSpec;

storyInputLocation: fileSpec | longOrShortSpaceSpec;
storyOutputLocation: fileSpec | longOrShortSpaceSpec;

storiesInputLocation: fileSpec | longOrShortSpaceSpec;
storiesOutputLocation: fileSpec | longOrShortSpaceSpec;

varGetFrom: ('on' 'server' | 'in' fileSpec);

updateBlocksStatement: 'update' 'blocks' 'in' longOrShortSpaceSpec 'set' blockUpdateList ('where' blockConstraintExprList)?;
copyBlocksStatement: 'copy' 'blocks' ('in' | 'from') longOrShortSpaceSpec 'to' blocksOutputLocation ('where' blockConstraintExprList)?;
deleteBlocksStatement: 'delete' 'blocks' ('in' | 'from') longOrShortSpaceSpec ('where' blockConstraintExprList)?;

blockConstraintExprList: blockConstraintExpr (('and' | 'or') blockConstraintExprList)?;

blockConstraintExpr: blockConstraint (('and' | 'or') blockConstraintExpr)?
	| '(' blockConstraint (('and' | 'or') blockConstraintExpr)? ')'
	| '(' blockConstraintExpr (('and' | 'or') blockConstraintExpr)? ')'
	;

blockConstraint: 'id' ('=' | '!=') intExpr
	| 'id' 'not'? 'in' '(' intExprList ')'
	| 'name' ('=' | '!=') stringExpr
	| 'name' 'not'? 'in' '(' stringExprList ')'
	| 'name' ('matches' | 'does' 'not' 'match') 'regex'? regexExpr
	| 'name' 'not'? 'in' '(' regexExprList ')'
	| 'name' 'not'? 'like' stringExpr
	| 'name' ('starts' | 'does' 'not' 'start') 'with' stringExpr
	| 'name' ('ends' | 'does' 'not' 'end') 'with' stringExpr
	;

blockUpdateList: blockUpdate (',' blockUpdateList)?;

blockUpdate: 'technical' 'name' '=' stringExpr
	| 'display' 'name' '=' stringExpr
	| 'type' '=' ('nestable' | 'content' | 'universal')
	| 'add' 'tag' stringExpr
	| 'remove' 'tag' stringExpr
	| 'preview' 'field' '=' stringExpr
	| 'preview' 'template' '=' stringExpr
	| 'preview' 'screenshot' '=' stringExpr
	;

intExprList: intExpr (',' intExprList)?;

intExpr: (INTLITERAL | VARID) (('+' | '-' | '*' | '%') intExpr)?;

verbosityStatement: 'be'? ('quiet' | 'verbose' | 'debugger');

waitStatement: 'wait' INTLITERAL;

compareStatement: compareSpacesStatement
	| compareBlocksStatement
	| compareAllBlocksStatement
	;

compareSpacesStatement: 'compare' spaceSpec 'and' spaceSpec;
compareBlocksStatement: 'compare' blockSpec 'and' blockSpec;
compareAllBlocksStatement: 'compare' 'all' 'blocks' 'in' spaceSpec 'and' spaceSpec;

copyStoriesStatement: 'copy' 'stories' ('with' 'content')? ('in' | 'from') storiesInputLocation 'to' storiesOutputLocation ('where' storyConstraintExprList)?;
publishStoriesStatement: 'publish' 'stories' ('in' | 'from') longOrShortSpaceSpec ('where' storyConstraintExprList)?;
unpublishStoriesStatement: 'unpublish' 'stories' ('in' | 'from') longOrShortSpaceSpec ('where' storyConstraintExprList)?;
deleteStoriesStatement: 'delete' 'stories' ('in' | 'from') longOrShortSpaceSpec ('where' storyConstraintExprList)?;

storyConstraintExprList: storyConstraintExpr (('and' | 'or') storyConstraintExprList)?;

storyConstraintExpr: storyConstraint (('and' | 'or') storyConstraintExpr)?
	| '(' storyConstraint (('and' | 'or') storyConstraintExpr)? ')'
	| '(' storyConstraintExpr (('and' | 'or') storyConstraintExpr)? ')'
	;

storyConstraint: 'id' ('=' | '!=') intExpr
	| 'id' 'not'? 'in' '(' intExprList ')'
	| ('name' | 'url') ('=' | '!=') stringExpr
	| ('name' | 'url') 'not'? 'in' '(' stringExprList ')'
	| ('name' | 'url') ('matches' | 'does' 'not' 'match') 'regex'? regexExpr
	| ('name' | 'url') 'not'? 'in' '(' stringExprList ')'
	| ('name' | 'url') 'not'? 'like' stringExpr
	| ('name' | 'url') ('starts' | 'does' 'not' 'start') 'with' stringExpr
	| ('name' | 'url') ('ends' | 'does' 'not' 'end') 'with' stringExpr
	| (('any'? 'tag') | ('all'? 'tags')) ('=' | '!=') stringExpr
	| (('any'? 'tag') | ('all'? 'tags')) 'not'? 'in' '(' stringExprList ')'
	| 'any'? 'tag' ('matches' | 'does' 'not' 'match') 'regex'? regexExpr
	| 'any'? 'tag' ('starts' | 'does' 'not' 'start') 'with' stringExpr
	| 'any'? 'tag' ('ends' | 'does' 'not' 'end') 'with' stringExpr
	| 'all'? 'tags' ('match' | 'do' 'not' 'match') 'regex'? regexExpr
	| 'all'? 'tags' ('start' | 'do' 'not' 'start') 'with' stringExpr
	| 'all'? 'tags' ('end' | 'do' 'not' 'end') 'with' stringExpr
	| (('any'? 'tag') | ('all'? 'tags')) 'not'? 'in' '(' regexExprList ')'
	| (('any'? 'tag') | ('all'? 'tags')) 'not'? 'like' stringExpr
	| 'no' 'tags'
	| 'any' 'tags'
	;

regexExpr: STRINGLITERAL | REGEXLITERAL | VARID;

regexExprList: regexExpr (',' regexExprList)?;

copyDatasourcesStatement: 'copy' 'datasources' ('from' | 'in') longOrShortSpaceSpec 'to' longOrShortSpaceSpec ('where' datasourceConstraintExprList)? datasourceCopyOptionList?;
updateDatasourcesStatement: 'update' 'datasources' ('from' | 'in') longOrShortSpaceSpec 'set' datasourceUpdateList ('where' datasourceConstraintExprList)?;
deleteDatasourcesStatement: 'delete' 'datasources' ('from' | 'in') longOrShortSpaceSpec ('where' datasourceConstraintExprList)?;
syncDatasourcesStatement: 'copy' 'datasources' ('from' | 'in') longOrShortSpaceSpec 'to' longOrShortSpaceSpec ('where' datasourceConstraintExprList)?;

datasourceCopyOptionList: datasourceCopyOption (',' datasourceCopyOptionList)?;
datasourceCopyOption: 'skip' ('update' | 'updates' | 'create' | 'creates')
	| 'include' 'entries'
	;

datasourceConstraintExprList: datasourceConstraintExpr (('and' | 'or') datasourceConstraintExprList)?;

datasourceConstraintExpr: datasourceConstraint (('and' | 'or') datasourceConstraintExpr)?
	| '(' datasourceConstraint (('and' | 'or') datasourceConstraintExpr)? ')'
	| '(' datasourceConstraintExpr (('and' | 'or') datasourceConstraintExpr)? ')'
	;

datasourceConstraint: 'id' ('=' | '!=') intExpr
	| 'id' 'not'? 'in' '(' intExprList ')'
	| ('name' | 'slug') ('=' | '!=') stringExpr
	| ('name' | 'slug') 'not'? 'in' '(' stringExprList ')'
	| ('name' | 'slug') ('matches' | 'does' 'not' 'match') 'regex'? regexExpr
	| ('name' | 'slug') 'not'? 'in' '(' regexExprList ')'
	| ('name' | 'slug') 'not'? 'like' stringExpr
	| ('name' | 'slug') ('starts' | 'does' 'not' 'start') 'with' stringExpr
	| ('name' | 'slug') ('ends' | 'does' 'not' 'end') 'with' (stringExpr)
	;

stringExprList: stringExpr (',' stringExprList)?;

stringExpr: (STRINGLITERAL | VARID | varFieldExpr | fnCallExpr) ('+' stringExpr)?
	| '(' (STRINGLITERAL | VARID | varFieldExpr) ('+' stringExpr)? ')'
	| '(' stringExpr ('+' stringExpr)? ')'
	;

varFieldExpr: VARID '[' stringExpr ']';

fnCallExpr: VARID '(' fnCallActualArgList? ')';

fnCallActualArgList: fnActualArg (',' fnCallActualArgList)?;

fnActualArg: (VARID '=')? stringExpr;

spaceConstraintExprList: spaceConstraintExpr (('and' | 'or') spaceConstraintExprList)?;

spaceConstraintExpr: spaceConstraint (('and' | 'or') spaceConstraintExpr)?
	| '(' spaceConstraint (('and' | 'or') spaceConstraintExpr)? ')'
	| '(' spaceConstraintExpr (('and' | 'or') spaceConstraintExpr)? ')'
	;

spaceConstraint: 'id' ('=' | '!=') intExpr
	| 'id' 'not'? 'in' '(' intExprList ')'
	| 'name' ('=' | '!=') stringExpr
	| 'name' 'not'? 'in' '(' stringExprList ')'
	| 'name' ('matches' | 'does' 'not' 'match') 'regex'? regexExpr
	| 'name' 'not'? 'in' '(' regexExprList ')'
	| 'name' 'not'? 'like' stringExpr
	| 'name' ('starts' | 'does' 'not' 'start') 'with' stringExpr
	| 'name' ('ends' | 'does' 'not' 'end') 'with' (stringExpr)
	;

datasourcesInputLocation: fileSpec | longOrShortSpaceSpec;
datasourcesOutputLocation: fileSpec | longOrShortSpaceSpec;

datasourceInputLocation: fileSpec | longOrShortSpaceSpec;
datasourceOutputLocation: fileSpec | longOrShortSpaceSpec;

dirSpec: 'directory' (STRINGLITERAL | VARID);

forEachStatement: 'foreach' '(' typedVarDecl 'in' foreachEntityListForTypedVarDecl ')' scriptBlockDef
	| 'foreach' '(' untypedVarDecl 'in' foreachEntityListForUntypedVarDecl ')' scriptBlockDef
	;

foreachEntityListForTypedVarDecl: foreachSpaceListForTypedVarDecl
	| foreachBlockListForTypedVarDecl
	| foreachDatasourceListForTypedVarDecl
	| foreachDatasourceEntryListForTypedVarDecl
	| foreachStoryListForTypedVarDecl
	| foreachStringListForTypedVarDecl
	| foreachRegexListForTypedVarDecl
	| foreachIntegerListForTypedVarDecl;

foreachEntityListForUntypedVarDecl: foreachSpaceListForUntypedVarDecl
	| foreachBlockListForUntypedVarDecl
	| foreachDatasourceListForUntypedVarDecl
	| foreachDatasourceEntryListForUntypedVarDecl
	| foreachStoryListForUntypedVarDecl
	| foreachStringListForUntypedVarDecl
	| foreachRegexListForUntypedVarDecl
	| foreachIntegerListForUntypedVarDecl;

foreachSpaceListForTypedVarDecl: (fileSpec | spaceFileSpec | 'spaces') ('where' spaceConstraintExprList)?;
foreachSpaceListForUntypedVarDecl: (spaceFileSpec | 'spaces') ('where' spaceConstraintExprList)?;

foreachBlockListForTypedVarDecl: (fileSpec | blockFileSpec | longOrShortSpaceSpec) ('where' blockConstraintExprList)?;
foreachBlockListForUntypedVarDecl: (blockFileSpec | 'blocks' 'in' spaceSpec) ('where' blockConstraintExprList)?;

foreachDatasourceListForTypedVarDecl: (fileSpec | datasourceFileSpec | longOrShortSpaceSpec) ('where' datasourceConstraintExprList)?;
foreachDatasourceListForUntypedVarDecl: (datasourceFileSpec | 'datasources' 'in' spaceSpec) ('where' datasourceConstraintExprList)?;

foreachDatasourceEntryListForTypedVarDecl: (fileSpec | datasourceEntryFileSpec | longOrShortDatasourceSpec) ('where' datasourceEntryConstraintExprList)?;
foreachDatasourceEntryListForUntypedVarDecl: (datasourceEntryFileSpec | longOrShortDatasourceSpec) ('where' datasourceEntryConstraintExprList)?;

foreachStoryListForTypedVarDecl: (fileSpec | storyFileSpec | spaceSpec) ('where' storyConstraintExprList)?;
foreachStoryListForUntypedVarDecl: (storyFileSpec | 'stories' 'in' spaceSpec) ('where' storyConstraintExprList)?;

foreachStringListForTypedVarDecl: fileSpec | 'string' fileSpec | stringArrayLiteral;
foreachStringListForUntypedVarDecl: 'string' fileSpec | stringArrayLiteral;

foreachRegexListForTypedVarDecl: fileSpec | 'regex' fileSpec | regexArrayLiteral;
foreachRegexListForUntypedVarDecl: 'regex' fileSpec | regexArrayLiteral;

foreachIntegerListForTypedVarDecl: fileSpec | 'int' fileSpec | intArrayLiteral;
foreachIntegerListForUntypedVarDecl: 'int' fileSpec | intArrayLiteral;

stringArrayLiteral: '[' stringExprList? ']';
regexArrayLiteral: '[' regexExprList? ']';
intArrayLiteral: '[' intExprList? ']';

longOrShortDatasourceSpec: datasourceSpec | datasourceShortSpec;

spaceFileSpec: 'space' fileSpec;
blockFileSpec: 'block' fileSpec;
datasourceFileSpec: 'datasource' fileSpec;
datasourceEntryFileSpec: 'datasource entry' fileSpec;
storyFileSpec: 'story' fileSpec;

untypedVarDecl: 'var' VARID;

typedVarDecl: spaceVarDecl
	| blockVarDecl
	| datasourceVarDecl
	| datasourceEntryVarDecl
	| storyVarDecl
	| stringVarDecl
	| regexVarDecl
	| integerVarDecl
	;

spaceVarDecl: 'space' VARID;
blockVarDecl: 'block' VARID;
datasourceVarDecl: 'datasource' VARID;
datasourceEntryVarDecl: 'datasource' 'entry' VARID;
storyVarDecl: 'story' VARID;
stringVarDecl: 'string' VARID;
regexVarDecl: 'regex' VARID;
integerVarDecl: 'int' VARID;

datasourceEntriesInputLocation: fileSpec | datasourceSpec;
