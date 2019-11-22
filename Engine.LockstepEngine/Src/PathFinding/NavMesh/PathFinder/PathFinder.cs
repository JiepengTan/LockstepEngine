// Copyright 2019 谭杰鹏. All Rights Reserved //https://github.com/JiepengTan 

namespace Lockstep.PathFinding {
	public interface PathFinder<N> {

		bool SearchPath(N startNode, N endNode, Heuristic<N> heuristic, GraphPath<Connection<N>> outPath);

		bool SearchNodePath(N startNode, N endNode, Heuristic<N> heuristic, GraphPath<N> outPath);
	}
}