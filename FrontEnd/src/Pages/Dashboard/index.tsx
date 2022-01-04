import { Grid } from "@mui/material";
import React, { useEffect, useState } from "react";
import MakeARecommendation from "../../Components/make-recommendation";
import PostFilter from "./Components/post-filter";
import SearchAutoComplete from "../../Components/search-bar";
import PostCard from "./Components/post-card";
import PostReplyCard from "./Components/post-reply-card";
import { IThreadDetails } from "../../Common/Interfaces";
import { Axios } from "../../Common";

const getAllThreads = async (userId?: number | null) => {
  const params = new URLSearchParams();

  if (userId) params.append("userId", userId.toString());

  const response = await Axios.get<IThreadDetails[]>(
    `thread?${params.toString()}`
  );
  const reponseData = response.data;
  return reponseData;
};

function Dashboard() {
  const [threads, setThreads] = useState<IThreadDetails[]>([]);

  const [selectedTab, setSelectedTab] = useState<number>(0);

  useEffect(() => {
    const userId = selectedTab === 0 ? 1 : null;
    getAllThreads(userId).then((response) => {
      setThreads(response);
    });
  }, [selectedTab]);

  console.log(threads);

  const threadList = threads.map((thread) => (
    <>
      <PostCard thread={thread} />

      {thread.posts &&
        thread.posts.slice(1).map((post) => (
          <>
            <PostReplyCard post={post} thread={thread} />
          </>
        ))}
    </>
  ));

  return (
    <Grid
      sx={{ height: "85vh", overflow: "auto" }}
      container
      direction="row"
      alignItems="center"
      gap="3%"
    >
      <MakeARecommendation />
      <Grid container flexDirection="row" justifyContent="space-around">
        <PostFilter selectedTab={selectedTab} setSelectedTab={setSelectedTab} />
        <SearchAutoComplete />
      </Grid>
      {threadList}
    </Grid>
  );
}
export default Dashboard;
