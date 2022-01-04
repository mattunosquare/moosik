import {
  Divider,
  FormControl,
  InputLabel,
  MenuItem,
  Select,
  TextField,
} from "@mui/material";
import Box from "@mui/material/Box";
import Typography from "@mui/material/Typography";
import { Button } from "@mui/material";
import { Grid } from "@mui/material";
import { Card } from "@mui/material";
import CardContent from "@mui/material/CardContent";
import {
  IThreadDetails,
  IReplyToThreadForm,
  IPostResourceType,
} from "../../../Common/Interfaces";
import {
  replyToThread,
  getAllPostResourceTypes,
} from "../../../Services/PostService";
import { useEffect, useState } from "react";

const style = {
  position: "absolute" as "absolute",
  top: "50%",
  left: "50%",
  transform: "translate(-50%, -50%)",
  width: 400,
  height: "70%",
  bgcolor: "background.paper",
  border: "2px solid #000",
  boxShadow: 24,
  p: 4,
};

interface IProps {
  thread: IThreadDetails;
}

const ReplyModal = ({ thread }: IProps) => {
  const [description, setDescription] = useState<string>("");
  const [resourceTitle, setResourceTitle] = useState<string>("");
  const [resourceValue, setResourceValue] = useState<string>("");
  const [resourceTypes, setResourceTypes] = useState<IPostResourceType[]>([]);
  const [selectedResourceType, setSelectedResourceType] = useState<string>("");
  const [selectedResourceTypeId, setSelectedResourceTypeId] =
    useState<number>(0);

  useEffect(() => {
    getAllPostResourceTypes().then((response) => {
      setResourceTypes(response);
    });
  }, []);

  const resourceTypeList = resourceTypes.map((type) => (
    <MenuItem key={type.id} value={type.description}>
      {type.description}
    </MenuItem>
  ));

  return (
    <div>
      <Box sx={style}>
        <Grid
          container
          direction="column"
          justifyContent="space-around"
          style={{ height: "100%" }}
        >
          <Grid item>
            <Card variant="elevation">
              <CardContent>
                <Typography variant="subtitle1">{thread.title}</Typography>
                <Typography variant="body1">
                  {thread.posts[0].description}
                </Typography>
                <Grid container justifyContent="flex-end" sx={{ mt: "10%" }}>
                  <Typography variant="body1">
                    by {thread.user.username} on {thread.createdDate}
                  </Typography>
                </Grid>
              </CardContent>
            </Card>
          </Grid>

          <Divider variant="middle" />

          <Grid item>
            <TextField
              id="post-reply-field"
              label="Post Reply Text"
              fullWidth
              multiline
              variant="filled"
              rows={2}
              onChange={(e) => {
                setDescription(e.target.value);
              }}
            />
          </Grid>

          <Grid item>
            <FormControl fullWidth>
              <InputLabel id="demo-simple-select-label">Type</InputLabel>
              <Select
                labelId="demo-simple-select-label"
                id="demo-simple-select"
                value={selectedResourceType}
                label="Type"
                onChange={(e) => {
                  setSelectedResourceType(e.target.value);
                  setSelectedResourceTypeId(
                    resourceTypes.find(
                      (element) => element.description === e.target.value
                    )?.id ?? 0
                  );
                }}
              >
                {resourceTypeList}
              </Select>
            </FormControl>
          </Grid>

          <Grid item>
            <TextField
              id="post-resources-title"
              label="Please provide a title."
              fullWidth
              variant="filled"
              onChange={(e) => {
                setResourceTitle(e.target.value);
              }}
            />
          </Grid>

          <Grid item>
            <TextField
              id="post-resources-link"
              label="Please provide a link."
              fullWidth
              variant="filled"
              onChange={(e) => {
                setResourceValue(e.target.value);
              }}
            />
          </Grid>

          <Grid container justifyContent="flex-end" gap="3%">
            <Button size="small">Cancel</Button>
            <Button
              size="small"
              onClick={() => {
                const formData: IReplyToThreadForm = {
                  ThreadId: thread.id,
                  Description: description,
                  PostResourceTitle: resourceTitle,
                  PostResourceValue: resourceValue,
                  PostResourceTypeId: selectedResourceTypeId,
                };

                replyToThread(formData);
              }}
            >
              Save
            </Button>
          </Grid>
        </Grid>
      </Box>
    </div>
  );
};

export default ReplyModal;
